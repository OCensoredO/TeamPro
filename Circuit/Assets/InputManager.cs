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
        // �ٱ����� ������
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gManager.TogglePlayer(Player.Outer);
            gManager.ToggleBackGround(1);
        }
        // ������ ����
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
        // �����̽� : ��ȣ�ۿ�
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
        // Ŭ�� �� �� ����(����ȭ �� ��)(�ϴ��� �Ⱦ�)
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
