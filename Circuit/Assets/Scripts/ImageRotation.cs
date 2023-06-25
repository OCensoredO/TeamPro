using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotation : MonoBehaviour
{
    public float rotSpeed = 5.0f;
    private float rot = 0.0f;

    void Update()
    {
        rot += rotSpeed * Time.deltaTime;
        transform.Rotate(0.0f, 0.0f, rot);
    }
}
