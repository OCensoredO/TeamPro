using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionCheck : MonoBehaviour
{
    public string m_collidedObjTag { get; private set; }
    public string m_SuccessScene;
    public string m_failScene;

    private void Start()
    {
        m_collidedObjTag = "NONE";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_collidedObjTag = collision.gameObject.tag;
        //if (collision.gameObject.tag == "Obstacle") SceneManager.LoadScene("Main");
        if (collision.CompareTag("Obstacle")) SceneManager.LoadScene(m_failScene);
        if (collision.CompareTag("Finish")) SceneManager.LoadScene(m_SuccessScene);
        if (collision.CompareTag("Wall")) return;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_collidedObjTag = "NONE";
    }

    //public bool IsCollidedWith(ref string objTag) { return m_collidedObjTag == objTag; }

    //public ref string returnCollidedObjectName(ref string name) { return ref name; }
}
