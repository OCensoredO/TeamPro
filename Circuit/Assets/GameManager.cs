using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    float radius;

    public const int Inner = 0;
    public const int Outer = 1;
    // 빠른 기능 구현 위해 임시로 obj를 public으로 설정함, 따라서 나중에 public 키워드는 뺄 예정
    public GameObject obj;

    public Player(GameObject obj)
    {
        this.obj = obj;
        this.radius = Vector2.Distance(Vector2.zero, obj.transform.position);
    }

    // moveCircular : 달린 거리값(runDistance)을 받아서 해당 위치로 이동 및 궤도에 알맞게 회전
    public void moveCircular(float runDistance)
    {
        obj.transform.position = RunDistanceToPos(runDistance);
        obj.transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 1) - obj.transform.position);
    }

    // RunDistanceToPos : float형의 달린 거리값(runDistance)을 Vector2 값으로 바꿔서 반환
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
    List<Player> m_players;                   // player 오브젝트들 저장하는 리스트
    List<GameObject> m_maps;
    //GameObject mapIn;
    //GameObject mapOut;

    private float runDistance;
    private float speed;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        gSquareObj = GameObject.FindGameObjectWithTag("GSquare");

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
        speed = 2.0f;
    }

    private void Update()
    {
        inputManager.ManageInput();

        foreach (Player obj in m_players)
        {
            obj.moveCircular(runDistance);
            //왕임시
            obj.obj.transform.localPosition = new Vector2(obj.obj.transform.localPosition.x, obj.obj.transform.localPosition.y + Mathf.Cos(Time.time) * 0.15f);
        }
    }

    public void Run(int direction)
    {
        // direction : 시계 방향 => 1 / 반시계 방향 => -1
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
