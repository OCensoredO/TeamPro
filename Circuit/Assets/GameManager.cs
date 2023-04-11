using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // ���� ��� ���� ���� �ӽ÷� obj�� public���� ������, ���� ���߿� public Ű����� �� ����
    public GameObject obj;
    float radius;
    
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
    private InputManager inputManager;
    private GameObject gSquareObj;
    public List<Player> players;                   // player ������Ʈ�� �����ϴ� ����Ʈ (0 : ����, 1 : �ٱ���)
    // originPos : Jump() ���� �ӽ� ����, ���߿� ������ ��ĥ ��
    Vector2 originPos;

    private float runDistance;
    private float speed;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        gSquareObj = GameObject.FindGameObjectWithTag("GSquare");
        players = new List<Player>();
        foreach (GameObject pObj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(new Player(pObj));
        }
        players[1].Toggle();

        runDistance = 0.0f;
        speed = 2.0f;
    }

    private void Update()
    {
        inputManager.ManageInput();

        foreach (Player obj in players)
        {
            obj.moveCircular(runDistance);
        }
    }

    public void Run(int direction)
    {
        // direction : �ð� ���� => 1 / �ݽð� ���� => -1
        runDistance += Time.deltaTime * speed * (float)direction;
    }

    public void TogglePlayer()
    {
        foreach (Player obj in players)
        {
            obj.Toggle();
        }
    }
}
