using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_collidedObjTag = "NONE";
    }

    //public bool IsCollidedWith(ref string objTag) { return m_collidedObjTag == objTag; }

    //public ref string returnCollidedObjectName(ref string name) { return ref name; }
}