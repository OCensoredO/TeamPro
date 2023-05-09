using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    float radius;

    public const int Inner = 0;
    public const int Outer = 1;
    // ���� ��� ���� ���� �ӽ÷� obj�� public���� ������, ���� ���߿� public Ű����� �� ����
    public GameObject obj;

    public Player(GameObject obj)
    {
        this.obj = obj;
        this.radius = Vector2.Distance(Vector2.zero, obj.transform.position);
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
        Vector2 pos = new Vector2(radius * Mathf.Cos(runDistance - Mathf.PI / 2.0f),
                                   radius * Mathf.Sin(runDistance - Mathf.PI / 2.0f));
        return pos;
    }

    public void Toggle()
    {
        this.obj.SetActive(!this.obj.activeSelf);
    }
}

public class GameManager : MonoBehaviour
{
    InputManager inputManager;
    GameObject gSquareObj;
    List<Player> m_players;                   // player ������Ʈ�� �����ϴ� ����Ʈ
    List<GameObject> m_maps;
    //GameObject mapIn;
    //GameObject mapOut;

    private float runDistance;
    private float speed;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        gSquareObj = GameObject.FindGameObjectWithTag("GSquare");

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

    public void ToggleBackGround(int mapIndex)
    {
        if (m_maps[mapIndex].gameObject.activeSelf) return;
        foreach (GameObject map in m_maps) map.gameObject.SetActive(!map.gameObject.activeSelf);
    }
}
