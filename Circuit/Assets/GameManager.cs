using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    // ���� ���� �߾ӿ��� ������ �Ÿ�, ���� �������� �ƴѵ� �ؼ� ���� ���ɼ� ����
    private float posByRadius;
    private Quaternion originalRotation;

    // ���� ���� ���� �� ��� ����
    public const int Inner = 0;
    public const int Outer = 1;

    // ���� ��� ���� ���� �ӽ÷� obj�� public���� ������, ���� ���߿� public Ű����� �� ����
    // ...�̾��µ� �Ʒ�ó�� �߰�ȣ �ȿ� �Ӽ� �ο�?�ϸ� ���Ȼ����ε� ����?����?��?
    public GameObject obj { get; private set; }

    public Player(GameObject obj)
    {
        this.obj = obj;
        this.posByRadius = Vector2.Distance(Vector2.zero, obj.transform.position);
        this.originalRotation = obj.transform.rotation;
    }

    // moveCircular : �޸� �Ÿ���(runDistance)�� �޾Ƽ� �ش� ��ġ�� �̵� �� �˵��� �˸°� ȸ��
    public void moveCircular(float runDistance)
    {
        // ��ġ ����
        obj.transform.position = RunDistanceToPos(runDistance);
        // �����̼� ����
        obj.transform.rotation = originalRotation * Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 1) - obj.transform.position);
        //obj.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 1) - obj.transform.position);
    }

    // RunDistanceToPos : float���� �޸� �Ÿ���(runDistance)�� Vector2 ������ �ٲ㼭 ��ȯ
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
    public List<Player> m_players { get; private set; }                   // player ������Ʈ�� �����ϴ� ����Ʈ
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

        // �÷��̾� ������Ʈ ã�Ƽ� �Ҵ��ϱ�
        m_players = new List<Player>();
        foreach (GameObject pObj in GameObject.FindGameObjectsWithTag("Player"))
            m_players.Add(new Player(pObj));
        m_players[Player.Outer].Toggle();                        // ���� ���ʿ������� ����

        // ��� ������Ʈ ã�Ƽ� �Ҵ��ϱ�
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
            // ������Ű��
            player.obj.transform.localPosition += player.obj.transform.up * Mathf.Sin(Time.time * floatingSpeed) * floatingHeight;
        }
    }

    public void Interact()
    {
        switch (GetActivePlayerComponent<CollisionCheck>().m_collidedObjTag)
        {
            case "Lever":
                Debug.Log("���� �ǵ帲");
                leverState = !leverState;
                break;
            default:
                break;
        }
    }

    // ������ �ϳ��� �ִ� �� �÷��̾� ������Ʈ ��, ���� Ȱ��ȭ��(�� ȭ�鿡 ���̴�) �÷��̾��� ���ϴ� ������Ʈ(T) ��ȯ
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
        // �� �浹 ó���� ���� ����
        float wallCollisionC = checkWalled ? -1.0f : 1.0f;

        // direction : �ð� ���� => 1 / �ݽð� ���� => -1
        runDistance += Time.deltaTime * speed * (float)direction * wallCollisionC;
        //runDistance += (float)direction * wallCollisionC * Time.deltaTime;
        GetActivePlayer().Flip(direction);
    }

    public void TogglePlayer(int playerIndex)
    {
        if (m_players[playerIndex].obj.activeSelf) return;
        foreach (Player player in m_players) player.Toggle();
    }

    // ��� ��ȯ
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
