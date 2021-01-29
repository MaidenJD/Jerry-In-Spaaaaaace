using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Debris : MonoBehaviour
{
    public DebrisEvent CollisionHit = new DebrisEvent();

    public Rigidbody2D rb { get; private set; }
    private bool attached = false;
    private FixedJoint2D attachedJoint = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Debris"))
        {
            CollisionHit.Invoke(this, collision.gameObject.GetComponent<Debris>());
        }
    }

    public void Attach(Rigidbody2D otherRB)
    {
        attachedJoint = gameObject.AddComponent<FixedJoint2D>();
        attachedJoint.connectedBody = otherRB;
        attached = true;
    }

    public void Detach()
    {
        attachedJoint.enabled = false;
        Destroy(attachedJoint);
        attached = false;
    }

    public class DebrisEvent : UnityEvent<Debris, Debris> { }
}
