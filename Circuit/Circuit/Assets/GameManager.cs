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
        // originPos : Jump() ���� �ӽ� ����, ���߿� ������ ��ĥ ��
        originPos = gSquareObj.transform.position;
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

        // i : ���� ��������� �ӽ� ���� / �ݵ�� ������ ��
        int i = -1;

        foreach (Player obj in players)
        {
            obj.moveCircular(runDistance);

            // ���� ���� �ڵ�� => ���߿� �� ������ ��!!!!!!!!!!!!
            Vector2 jumpPos = gSquareObj.transform.position;
            Vector2 jumpVec = jumpPos - originPos;

            Vector2 jumpDirection = new Vector2(i * obj.obj.transform.position.x, i * obj.obj.transform.position.y);
            Vector2 totJumpVec = jumpDirection.normalized * jumpVec.magnitude;
            obj.moveCircular(runDistance);
            obj.obj.transform.position += (Vector3)totJumpVec;
            Debug.Log(jumpVec.magnitude);
            i = 1;
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

    // Jump() : ���� ����� ������Ʈ�� ���� ���� �޼ҵ� / ���� �ٸ� Ŭ������ ��ġ�� ���ɼ� ����
    // ���� ���� ���� ���߿� ���� �� ��
    public void Jump()
    {
        // ó�� �����ϱ� �� ��ġ���� �󸶳� �پ����� �˾ƿͼ� �װŷ� ��¦��¦�� ����
        gSquareObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200.0f));
        
    }
}
