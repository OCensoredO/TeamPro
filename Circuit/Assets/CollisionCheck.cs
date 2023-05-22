using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionCheck : MonoBehaviour
{
    public string m_collidedObjTag { get; private set; }

    private void Start()
    {
        m_collidedObjTag = "NONE";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_collidedObjTag = collision.gameObject.tag;
        if (collision.gameObject.tag == "Obstacle") SceneManager.LoadScene("Main");
        if (collision.gameObject.tag == "Finish") SceneManager.LoadScene("Test");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_collidedObjTag = "NONE";
    }

    //public bool IsCollidedWith(ref string objTag) { return m_collidedObjTag == objTag; }

    //public ref string returnCollidedObjectName(ref string name) { return ref name; }
}
