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

    // �÷��̾� ������Ʈ
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

    public List<Player> m_players { get; private set; }                   // player ������Ʈ�� �����ϴ� ����Ʈ
    public List<GameObject> m_maps { get; private set; }

    public GameObject prefab;
    // �� ��������Ʈ ����� ��������Ʈ �迭
    public Sprite[] InnerSprites;
    public Sprite[] OuterSprites;

    private float runDistance;
    private float speed;
    //private float frameCounter;
    //public float frameCounterFlag { get; private set; }
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
        m_maps.Add(GameObject.FindGameObjectWithTag("InnerMap"));
        m_maps.Add(GameObject.FindGameObjectWithTag("OuterMap"));

        // ��� �⺻ ��������Ʈ ����, �ȿ��� ������ ���̹Ƿ� �ȿ� ���� �� ���� ��������Ʈ(0�� �ε����� ��������Ʈ)�� �⺻��
        ToggleBackGround(0);

        runDistance = 0.0f;
        speed = 2.0f;
        //frameCounter = 0.0f;
        leverState = false;
        inCollision = false;

        // ������Ʈ �ʿ� �޶�ٰ� �ϴ� �� �׽�Ʈ��
        //Instantiate(prefab, m_maps[0].transform);
    }

    private void Update()
    {
        m_inputManager.ManageInput();

        foreach (Player player in m_players)
        {
            player.moveCircular(runDistance);
            // ������Ű��
            player.obj.transform.localPosition += player.obj.transform.up * Mathf.Sin(Time.time * 1.5f) * 0.05f;
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
        float wallCollisionC = checkWalled ? -3.0f : 1.0f;

        // direction : �ð� ���� => 1 / �ݽð� ���� => -1
        runDistance += Time.deltaTime * speed * (float)direction * wallCollisionC;
        GetActivePlayer().Flip(direction);
    }

    public void TogglePlayer(int playerIndex)
    {
        if (m_players[playerIndex].obj.activeSelf) return;
        foreach (Player player in m_players) player.Toggle();
    }

    // ��� ��ȯ(mapIndex : 0 = ��, 1 = ��)
    // SetActive���� �ٲٴ� ��Ŀ��� ��������Ʈ�� ���Ƴ��� ������� �ٲ�
    public void ToggleBackGround(int mapIndex)
    {
        // ��������Ʈ ���Ƴ���
        m_maps[0].GetComponent<SpriteRenderer>().sprite = InnerSprites[mapIndex];
        m_maps[1].GetComponent<SpriteRenderer>().sprite = OuterSprites[mapIndex];
        // ��������Ʈ ũ�� ������
        m_maps[0].transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        m_maps[1].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    public void RotateMap(int direction)
    {
        m_maps[0].transform.Rotate(0, 0, (float)direction * Time.deltaTime * 90.0f, Space.Self);
    }

    /*
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
    */
}
