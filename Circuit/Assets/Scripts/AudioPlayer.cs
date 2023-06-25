using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private GameObject[] bgms;
    
    void Start()
    {
        bgms = GameObject.FindGameObjectsWithTag("BGM");
        if (bgms.Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
    }
}
