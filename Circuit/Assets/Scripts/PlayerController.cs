using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private float jumpForce;
    //private Rigidbody2D rd;
    private float radius;
    public PlayerMove pMove;
    public PlayerToggle pToggle;

    void Start()
    {
        //jumpForce = 1.0f;
        //rd = GetComponent<Rigidbody2D>();
        radius = Vector2.Distance(Vector2.zero, transform.position);
        pMove = GameObject.Find("GameManager").GetComponent<PlayerMove>();
        pToggle = GameObject.Find("GameManager").GetComponent<PlayerToggle>();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pMove.Run(1);
            moveCircular();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            pMove.Run(-1);
            moveCircular();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            pToggle.Toggle(true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            pToggle.Toggle(false);
        }
        */
    }

    public void moveCircular()
    {
        transform.position = pMove.RunDistanceToPos(radius);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, new Vector3(0, 0, 1) - transform.position);
        Debug.Log(gameObject.name + pMove.RunDistanceToPos(radius));
    }
}
