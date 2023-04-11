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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gManager.TogglePlayer();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            gManager.TogglePlayer();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            gManager.Run(1);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            gManager.Run(-1);
        }
    }
}
