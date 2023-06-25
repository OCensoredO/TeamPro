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

    // 플레이어 오브젝트
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

    public List<Player> m_players { get; private set; }                   // player 오브젝트들 저장하는 리스트
    public List<GameObject> m_maps { get; private set; }
    public List<GameObject> m_leverObjs { get; private set; }

    public GameObject[] prefabs;

    // 스프라이트 저장할 배열들
    // 맵 스프라이트 변경용 스프라이트 배열
    public Sprite[] InnerSprites;
    public Sprite[] OuterSprites;
    // 오브젝트용 스프라이트
    public Sprite[] leverObjSprites;

    private float runDistance;
    private float speed;

    public bool leverState { get; private set; }
    public bool goalState { get; private set; }
    public int kioskCount;
    public int kioskNum { get; private set; }

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
        m_maps.Add(GameObject.FindGameObjectWithTag("InnerMap"));
        m_maps.Add(GameObject.FindGameObjectWithTag("OuterMap"));

        // 레버 오브젝트 찾아서 m_leverObjs에 저장
        m_leverObjs = new List<GameObject>();
        foreach (GameObject lObj in GameObject.FindGameObjectsWithTag("LeverObj"))
            m_leverObjs.Add(lObj);
        foreach (GameObject lObj in m_leverObjs)
            lObj.GetComponent<SpriteRenderer>().sprite = leverObjSprites[0];

        // 키오스크 갯수 세기
        foreach (object kiosk in GameObject.FindGameObjectsWithTag("Kiosk"))
            kioskNum++;

        // 배경 기본 스프라이트 설정, 안에서 시작할 것이므로 안에 있을 때 기준 스프라이트(0번 인덱스의 스프라이트)가 기본값
        ToggleBackGround(0);

        runDistance = 0.0f;
        speed = 2.0f;
        //frameCounter = 0.0f;
        leverState = false;
        goalState = kioskNum == 0 ? true : false;
        //inCollision = false;

        // 오브젝트 맵에 달라붙게 하는 거 테스트용
        //Instantiate(prefab, m_maps[0].transform);
    }

    private void Update()
    {
        m_inputManager.ManageInput();

        foreach (Player player in m_players)
        {
            player.moveCircular(runDistance);
            // 부유시키기
            player.obj.transform.localPosition += player.obj.transform.up * Mathf.Sin(Time.time * 1.5f) * 0.05f;
        }
    }

    public void Interact()
    {
        switch (GetActivePlayerComponent<CollisionCheck>().m_collidedObjTag)
        {
            case "Lever":
                leverState = !leverState;
                List<GameObject> TempleverObjs = new List<GameObject>();
                GameObject.FindGameObjectWithTag("Lever").GetComponent<SpriteRenderer>().flipX = !GameObject.FindGameObjectWithTag("Lever").GetComponent<SpriteRenderer>().flipX;

                foreach (GameObject lObj in m_leverObjs)
                {
                    GameObject clonedLObj;
                    if (leverState)
                    {
                        clonedLObj = Instantiate(prefabs[1], lObj.transform.position, lObj.transform.rotation);
                    }
                    else
                    {
                        clonedLObj = Instantiate(prefabs[0], m_maps[0].transform);
                        clonedLObj.transform.position = lObj.transform.position;
                        clonedLObj.transform.rotation = lObj.transform.rotation;
                        clonedLObj.transform.localScale = prefabs[0].transform.localScale;
                    }

                    clonedLObj.GetComponent<SpriteRenderer>().sortingLayerName = "Layer2";
                    TempleverObjs.Add(clonedLObj);
                    Destroy(lObj);
                }
                m_leverObjs.Clear();
                foreach (GameObject lObj in TempleverObjs)
                    m_leverObjs.Add(lObj);
                TempleverObjs.Clear();

                break;
            case "Kiosk":
                kioskCount++;
                if (kioskCount == kioskNum)
                {
                    GameObject clonedObj, originObj;
                    clonedObj = Instantiate(prefabs[2], m_maps[0].transform);
                    originObj = GameObject.FindGameObjectWithTag("ClosedGoal");
                    clonedObj.transform.position = originObj.transform.position;
                    clonedObj.transform.rotation = originObj.transform.rotation;
                    clonedObj.transform.localScale = prefabs[2].transform.localScale;
                    Destroy(originObj);

                    clonedObj = Instantiate(prefabs[3], m_maps[0].transform);
                    originObj = GameObject.FindGameObjectWithTag("Kiosk");
                    clonedObj.transform.position = originObj.transform.position;
                    clonedObj.transform.rotation = originObj.transform.rotation;
                    clonedObj.transform.localScale = prefabs[3].transform.localScale;
                    Destroy(originObj);
                }

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
        bool checkWalled = GetActivePlayerComponent<CollisionCheck>().m_collidedObjTag == "Wall" || GetActivePlayerComponent<CollisionCheck>().m_collidedObjTag == "LeverObj";
        // 벽 충돌 처리용 변수 선언
        float wallCollisionC = checkWalled ? -3.0f : 1.0f;

        // direction : 시계 방향 => 1 / 반시계 방향 => -1
        runDistance += Time.deltaTime * speed * (float)direction * wallCollisionC;
        GetActivePlayer().Flip(direction);
    }

    public void TogglePlayer(int playerIndex)
    {
        if (m_players[playerIndex].obj.activeSelf) return;
        foreach (Player player in m_players) player.Toggle();
    }

    // 배경 전환(mapIndex : 0 = 안, 1 = 밖)
    // SetActive값을 바꾸는 방식에서 스프라이트를 갈아끼는 방식으로 바꿈
    public void ToggleBackGround(int mapIndex)
    {
        // 스프라이트 갈아끼기
        m_maps[0].GetComponent<SpriteRenderer>().sprite = InnerSprites[mapIndex];
        m_maps[1].GetComponent<SpriteRenderer>().sprite = OuterSprites[mapIndex];
        // 스프라이트 크기 재조정
        //m_maps[0].transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        //m_maps[1].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void RotateMap(int direction)
    {
        m_maps[0].transform.Rotate(0, 0, (float)direction * Time.deltaTime * 90.0f, Space.Self);
    }

    /*
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
    */
}
