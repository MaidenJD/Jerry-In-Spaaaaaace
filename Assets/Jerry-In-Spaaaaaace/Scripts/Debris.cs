using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Debris : MonoBehaviour
{
    /// <summary>
    /// This Debris Instance, the debris it has hit, the world point of the hit
    /// </summary>
    public DebrisHitEvent CollisionHit = new DebrisHitEvent();

    public Rigidbody2D rb { get; private set; }
    private bool attached = false;
    private FixedJoint2D attachedJoint = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.Sleep();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < collision.contactCount; i++)
        {
            if(collision.contacts[i].collider.gameObject.CompareTag("Debris"))
            {
                CollisionHit.Invoke(this, collision.gameObject.GetComponent<Debris>(), collision.contacts[i].point);
                break;
            }
        }
    }

    public void Attach(Rigidbody2D otherRB, Vector2 hitPoint)
    {
        if(attached)
        {
            return;
        }

        rb.velocity = Vector2.zero;

        attachedJoint = gameObject.AddComponent<FixedJoint2D>();
        attachedJoint.connectedBody = otherRB;
        attached = true;

        hitPoint = transform.InverseTransformPoint(hitPoint);
        attachedJoint.anchor = hitPoint;
        attachedJoint.breakForce = 1000f;
        attachedJoint.breakTorque = 1000f;
    }

    public void Detach()
    {
        if(!attached)
        {
            return;
        }

        attachedJoint.enabled = false;
        Destroy(attachedJoint);
        attached = false;
    }

    public class DebrisHitEvent : UnityEvent<Debris, Debris, Vector2> { }
}
