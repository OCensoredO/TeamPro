using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    private GameManager gManager;

    void Start()
    {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void ManageInput()
    {
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
            gManager.Move(1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gManager.Move(-1);
        }
        // 스페이스 : 상호작용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gManager.Interact();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gManager.RotateCamera(-1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            gManager.RotateCamera(1);
        }
        // 클릭 시 맵 반전(최적화 안 됨)(일단은 안씀)
        /*
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
        */
    }
}
