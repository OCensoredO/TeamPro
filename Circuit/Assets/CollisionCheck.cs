using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionCheck : MonoBehaviour
{
    public string m_collidedObjTag { get; private set; }
    public Vector2 m_normal { get; private set; }

    private void Start()
    {
        m_collidedObjTag = "NONE";
        m_normal = new Vector2(0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_collidedObjTag = collision.gameObject.tag;
        
        if (collision.gameObject.CompareTag("Obstacle"))    SceneManager.LoadScene("Main");
        else if (collision.gameObject.CompareTag("Finish")) SceneManager.LoadScene("Test");
        //else if (collision.gameObject.CompareTag("Wall"))   m_normal = collision.ClosestPoint(collision.transform.position);
        //if (collision.gameObject.tag == "Obstacle") SceneManager.LoadScene("Main");
        //if (collision.gameObject.tag == "Finish") SceneManager.LoadScene("Test");
        //if (collision.gameObject.tag == "Wall")
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_collidedObjTag = "NONE";
    }

    //public bool IsCollidedWith(ref string objTag) { return m_collidedObjTag == objTag; }

    //public ref string returnCollidedObjectName(ref string name) { return ref name; }
}
