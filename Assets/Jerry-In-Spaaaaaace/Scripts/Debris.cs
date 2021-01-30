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

    public DebrisEvent JointBroken = new DebrisEvent();

    public ParticleSystem Thruster;

    /// <summary>
    /// The Rigidbody of this Debris
    /// </summary>
    public Rigidbody2D rb { get; private set; }
    /// <summary>
    /// Is this Debris attached to something
    /// </summary>
    private bool attached = false;
    /// <summary>
    /// The Joint that links this debre to the playership or another peace of debris
    /// </summary>
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

        var otherDebris = otherRB.GetComponent<Debris>();
        if(otherDebris != null)
        {
            otherDebris.JointBroken.AddListener(OnAttachedDebrisBroken);
        }
    }

    public void Detach()
    {
        if(!attached)
        {
            return;
        }

        var otherDebris = attachedJoint.connectedBody.GetComponent<Debris>();
        if(otherDebris != null)
        {
            otherDebris.JointBroken.RemoveListener(OnAttachedDebrisBroken);
        }

        attachedJoint.enabled = false;
        Destroy(attachedJoint);
        attached = false;
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        JointBroken.Invoke(this);
    }

    private void OnAttachedDebrisBroken(Debris attachedDebris)
    {
        JointBroken.Invoke(this);
    }

    public class DebrisHitEvent : UnityEvent<Debris, Debris, Vector2> { }

    public class DebrisEvent : UnityEvent<Debris> { }
}
