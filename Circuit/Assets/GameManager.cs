using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    // 원형 맵의 중앙에서 떨어진 거리, 좋은 변수명은 아닌듯 해서 수정 가능성 높음
    private float posByRadius;
    private Quaternion originalRotation;

    // 안팎 구분 위해 쓸 상수 설정
    public const int Inner = 0;
    public const int Outer = 1;

    // 빠른 기능 구현 위해 임시로 obj를 public으로 설정함, 따라서 나중에 public 키워드는 뺄 예정
    // ...이었는데 아래처럼 중괄호 안에 속성 부여?하면 보안상으로도 문제?없는?듯?
    public GameObject obj { get; private set; }

    public Player(GameObject obj)
    {
        this.obj = obj;
        this.posByRadius = Vector2.Distance(Vector2.zero, obj.transform.position);
        this.originalRotation = obj.transform.rotation;
    }

    // moveCircular : 달린 거리값(runDistance)을 받아서 해당 위치로 이동 및 궤도에 알맞게 회전
    public void moveCircular(float runDistance)
    {
        // 위치 설정
        obj.transform.position = RunDistanceToPos(runDistance);
        // 로테이션 설정
        obj.transform.rotation = originalRotation * Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 1) - obj.transform.position);
        //obj.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 1) - obj.transform.position);
    }

    // RunDistanceToPos : float형의 달린 거리값(runDistance)을 Vector2 값으로 바꿔서 반환
    public Vector2 RunDistanceToPos(float runDistance)
    {
        Vector2 pos = new Vector2(posByRadius * Mathf.Cos(runDistance - Mathf.PI / 2.0f),
                                   posByRadius * Mathf.Sin(runDistance + Mathf.PI / 2.0f));
        return pos;
    }

    public void Toggle()
    {
        this.obj.SetActive(!this.obj.activeSelf);
    }

    public void Flip(int direction)
    {
        this.obj.GetComponent<SpriteRenderer>().flipX = (direction == 1);
    }
}

public class GameManager : MonoBehaviour
{
    public float FPS { get; private set; }
    //public int boolToDirection(bool b) { return b ? 1 : -1; }

    private InputManager m_inputManager;
    private GameObject m_camera;
    //GameObject gSquareObj;
    public List<Player> m_players { get; private set; }                   // player 오브젝트들 저장하는 리스트
    public List<GameObject> m_maps { get; private set; }

    //GameObject mapIn;
    //GameObject mapOut;

    private float runDistance;
    private float speed;
    private float floatingSpeed;
    private float floatingHeight;
    //private float frameCounter;
    public float frameCounterFlag { get; private set; }
    public bool leverState { get; private set; }
    public bool inCollision;

    void Start()
    {
        FPS = 1.0f / Time.deltaTime;

        m_inputManager = gameObject.GetComponent<InputManager>();
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");

        // 플레이어 오브젝트 찾아서 할당하기
        m_players = new List<Player>();
        foreach (GameObject pObj in GameObject.FindGameObjectsWithTag("Player"))
            m_players.Add(new Player(pObj));
        m_players[Player.Outer].Toggle();                        // 원의 안쪽에서부터 시작

        // 배경 오브젝트 찾아서 할당하기
        m_maps = new List<GameObject>();
        foreach (GameObject mapObj in GameObject.FindGameObjectsWithTag("Map"))
            m_maps.Add(mapObj);
        m_maps[1].gameObject.SetActive(false);

        runDistance = 0.0f;
        speed = 1.8f;
        //frameCounter = 0.0f;
        floatingSpeed = 1.5f;
        floatingHeight = 0.06f;

        leverState = false;
        inCollision = false;
    }

    private void Update()
    {
        m_inputManager.ManageInput();

        foreach (Player player in m_players)
        {
            player.moveCircular(runDistance);
            // 부유시키기
            player.obj.transform.localPosition += player.obj.transform.up * Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        }
    }

    public void Interact()
    {
        switch (GetActivePlayerComponent<CollisionCheck>().m_collidedObjTag)
        {
            case "Lever":
                Debug.Log("레버 건드림");
                leverState = !leverState;
                break;
            default:
                break;
        }
    }

    // 안팎의 하나씩 있는 두 플레이어 오브젝트 중, 현재 활성화된(즉 화면에 보이는) 플레이어의 원하는 컴포넌트(T) 반환
    public T GetActivePlayerComponent<T>()
    {
        return m_players.FirstOrDefault(p => p.obj.activeSelf).obj.GetComponent<T>();
    }

    public Player GetActivePlayer()
    {
        return m_players.FirstOrDefault(p => p.obj.activeSelf);
    }

    public void Move(int direction)
    {
        bool checkWalled = GetActivePlayerComponent<CollisionCheck>().m_collidedObjTag == "Wall";
        // 벽 충돌 처리용 변수 선언
        float wallCollisionC = checkWalled ? -1.0f : 1.0f;

        // direction : 시계 방향 => 1 / 반시계 방향 => -1
        runDistance += Time.deltaTime * speed * (float)direction * wallCollisionC;
        //runDistance += (float)direction * wallCollisionC * Time.deltaTime;
        GetActivePlayer().Flip(direction);
    }

    public void TogglePlayer(int playerIndex)
    {
        if (m_players[playerIndex].obj.activeSelf) return;
        foreach (Player player in m_players) player.Toggle();
    }

    // 배경 전환
    public void ToggleBackGround(int mapIndex)
    {
        if (m_maps[mapIndex].gameObject.activeSelf) return;
        foreach (GameObject map in m_maps) map.gameObject.SetActive(!map.gameObject.activeSelf);
    }

    public void RotateCamera(int direction)
    {
        m_camera.transform.Rotate(0, 0, (float)direction * Time.deltaTime * 90.0f, Space.Self);
    }

    public void Bash()
    {

    }
}
