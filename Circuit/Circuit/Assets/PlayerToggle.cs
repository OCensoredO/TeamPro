using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToggle : MonoBehaviour
{
    private GameObject OuterPlayerObj;
    private GameObject InnerPlayerObj;

    void Start()
    {
        OuterPlayerObj = GameObject.Find("OuterPlayer");
        InnerPlayerObj = GameObject.Find("InnerPlayer");
        InnerPlayerObj.SetActive(false);
    }

    /*
    public void Toggle(bool isOuter)
    {
        OuterPlayerObj.SetActive(isOuter);
        InnerPlayerObj.SetActive(!isOuter);
        OuterPlayerObj.GetComponent<PlayerController>().moveCircular();
        InnerPlayerObj.GetComponent<PlayerController>().moveCircular();
    }
    */

    public void Toggle(ref GameObject obj)
    {
        obj.SetActive(obj.activeSelf);
    }
}
