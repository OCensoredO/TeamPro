using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private float runDistance;
    private float speed;
    private PlayerController pController;

    void Start()
    {
        runDistance = 0.0f;
        speed = 2.0f;
    }

    public void Run(int direction)
    {
        // direction : 시계 방향 => 1 / 반시계 방향 => -1
        runDistance += Time.deltaTime * speed * (float)direction;
    }

    public Vector2 RunDistanceToPos(float radius)
    {
        Vector2 pos = new Vector2(radius * Mathf.Cos(runDistance - Mathf.PI / 2.0f),
                                   radius * Mathf.Sin(runDistance - Mathf.PI / 2.0f));
        return pos;
    }
}
