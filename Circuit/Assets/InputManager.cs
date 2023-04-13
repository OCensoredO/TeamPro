using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameManager gManager;

    void Start()
    {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ManageInput()
    {
        // 의도하는 바로는 위, 아래 방향키 입력 시 각각 밖, 안으로 전환되어야 하나, 임시로 아무 키나 눌러도 반대쪽으로 전환되게 함
        // 바깥으로 나가기
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gManager.TogglePlayer(Player.Outer);
            gManager.ToggleBackGround(1);
        }
        // 안으로 들어가기
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gManager.TogglePlayer(Player.Inner);
            gManager.ToggleBackGround(0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gManager.Run(1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gManager.Run(-1);
        }
        // 클릭 시 맵 반전(최적화 안 됨)
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D mouseHit = Physics2D.Raycast(worldPoint, Vector2.zero);

            if (mouseHit.collider.name == "CircleArea")
            {
                gManager.ToggleBackGround(0);
            }
            else if (mouseHit.collider.name == "OuterArea")
            {
                gManager.ToggleBackGround(1);
            }
        }
    }
}
