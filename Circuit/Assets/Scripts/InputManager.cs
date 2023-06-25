using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputManager : MonoBehaviour
{
    private GameManager gManager;
    private AudioSource aSourse;
    public AudioClip[] wavs;

    void Start()
    {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        aSourse = GetComponent<AudioSource>();
    }

    public void ManageInput()
    {
        // �ٱ����� ������
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gManager.TogglePlayer(Player.Outer);
            gManager.ToggleBackGround(1);
            aSourse.clip = wavs[0];
            aSourse.Play();
        }
        // ������ ����
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gManager.TogglePlayer(Player.Inner);
            gManager.ToggleBackGround(0);
            aSourse.clip = wavs[1];
            aSourse.Play();
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
            switch (gManager.GetActivePlayerComponent<CollisionCheck>().m_collidedObjTag)
            {
                case "Lever":
                    aSourse.clip = wavs[2];
                    break;
                case "Kiosk":
                    aSourse.clip = wavs[3];
                    break;
                default:
                    break;
            }
            aSourse.Play();
        }
        // q, e : �� ������
        if (Input.GetKey(KeyCode.Q))
        {
            gManager.RotateMap(-1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            gManager.RotateMap(1);
        }
    }
}
