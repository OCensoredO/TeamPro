using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // 빠른 기능 구현 위해 임시로 obj를 public으로 설정함, 따라서 나중에 public 키워드는 뺄 예정
    public GameObject obj;
    float radius;
    
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
    private InputManager inputManager;
    private GameObject gSquareObj;
    public List<Player> players;                   // player 오브젝트들 저장하는 리스트 (0 : 안쪽, 1 : 바깥쪽)
    // originPos : Jump() 위한 임시 변수, 나중에 적당히 고칠 것
    Vector2 originPos;

    private float runDistance;
    private float speed;

    void Start()
    {
        inputManager = gameObject.GetComponent<InputManager>();
        gSquareObj = GameObject.FindGameObjectWithTag("GSquare");
        // originPos : Jump() 위한 임시 변수, 나중에 적당히 고칠 것
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

        // i : 점프 제어용으로 임시 선언 / 반드시 수정할 것
        int i = -1;

        foreach (Player obj in players)
        {
            obj.moveCircular(runDistance);

            // 점프 관련 코드들 => 나중에 꼭 수정할 것!!!!!!!!!!!!
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
        // direction : 시계 방향 => 1 / 반시계 방향 => -1
        runDistance += Time.deltaTime * speed * (float)direction;
    }

    public void TogglePlayer()
    {
        foreach (Player obj in players)
        {
            obj.Toggle();
        }
    }

    // Jump() : 물리 연산용 오브젝트를 띄우기 위한 메소드 / 추후 다른 클래스에 배치할 가능성 높음
    // 지역 변수 등은 나중에 고쳐 쓸 것
    public void Jump()
    {
        // 처음 점프하기 전 위치에서 얼마나 뛰었나를 알아와서 그거로 쿵짝쿵짝할 예정
        gSquareObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200.0f));
        
    }
}
