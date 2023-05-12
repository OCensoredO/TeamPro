using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player
{
    // ���� ���� �߾ӿ��� ������ �Ÿ�, ���� �������� �ƴѵ� �ؼ� ���� ���ɼ� ����
    private float posByRadius;

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
    }

    // moveCircular : �޸� �Ÿ���(runDistance)�� �޾Ƽ� �ش� ��ġ�� �̵� �� �˵��� �˸°� ȸ��
    public void moveCircular(float runDistance)
    {
        obj.transform.position = RunDistanceToPos(runDistance);
        obj.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 1) - obj.transform.position);
    }

    // RunDistanceToPos : float���� �޸� �Ÿ���(runDistance)�� Vector2 ������ �ٲ㼭 ��ȯ
    public Vector2 RunDistanceToPos(float runDistance)
    {
        Vector2 pos = new Vector2(posByRadius * Mathf.Cos(runDistance - Mathf.PI / 2.0f),
                                   posByRadius * Mathf.Sin(runDistance - Mathf.PI / 2.0f));
        return pos;
    }

    public void Toggle()
    {
        this.obj.SetActive(!this.obj.activeSelf);
    }

    public void Interact()
    {
        switch (obj.GetComponent<BoxCollider2D>().tag)
        {
            case "Lever":
                break;
            default:
                break;
        }
    }
}

public class GameManager : MonoBehaviour
{
    private InputManager inputManager;
    //GameObject gSquareObj;
    public List<Player> m_players { get; private set; }                   // player ������Ʈ�� �����ϴ� ����Ʈ
    public List<GameObject> m_maps { get; private set; }

    //GameObject mapIn;
    //GameObject mapOut;

    private float runDistance;
    private float speed;
    bool leverState;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        //gSquareObj = GameObject.FindGameObjectWithTag("GSquare");

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
        speed = 2.0f;
        leverState = false;
    }

    private void Update()
    {
        inputManager.ManageInput();

        foreach (Player player in m_players)
        {
            player.moveCircular(runDistance);
            // ������Ű��(���ӽ�)
            player.obj.transform.localPosition += player.obj.transform.up * Mathf.Sin(Time.time * 1.5f) * 0.12f;
        }
    }

    // ������ �ϳ��� �ִ� �� �÷��̾� ������Ʈ ��, ���� Ȱ��ȭ��(�� ȭ�鿡 ���̴�) �÷��̾��� ���ϴ� ������Ʈ ��ȯ
    public T GetActivePlayerComponent<T>()
    {
        return m_players.FirstOrDefault(p => p.obj.activeSelf).obj.GetComponent<T>();
    }

    public void Run(int direction)
    {
        // direction : �ð� ���� => 1 / �ݽð� ���� => -1
        runDistance += Time.deltaTime * speed * (float)direction;
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
}
