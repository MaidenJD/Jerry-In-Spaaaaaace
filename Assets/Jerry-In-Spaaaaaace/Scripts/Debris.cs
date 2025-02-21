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

    public DebrisEvent Attached = new DebrisEvent();

    public DebrisEvent Detached = new DebrisEvent();

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

    /// <summary>
    /// How far down the chain is this peace of debris
    /// </summary>
    public int chainCount = -1;

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
                if (collision.gameObject.GetComponent<Pickup>() == null)
                {
                    CollisionHit.Invoke(this, collision.gameObject.GetComponent<Debris>(), collision.contacts[i].point);
                    break;
                }
            }
        }
    }

    public void Attach(Rigidbody2D otherRB, Vector2 hitPoint)
    {
        if(attached)
        {
            return;
        }

        var otherDebris = otherRB.GetComponent<Debris>();
        if (otherDebris != null)
        {
            otherDebris.JointBroken.AddListener(OnAttachedDebrisBroken);

            chainCount = otherDebris.chainCount + 1;
        }
        else
        {
            //We are attaching to the ship
            chainCount = 1;
        }

        rb.velocity = Vector2.zero;

        attachedJoint = gameObject.AddComponent<FixedJoint2D>();
        attachedJoint.connectedBody = otherRB;
        attached = true;

        hitPoint = transform.InverseTransformPoint(hitPoint);
        attachedJoint.anchor = hitPoint;
        attachedJoint.breakForce = 100f / chainCount;
        attachedJoint.breakTorque = 100f / chainCount;

        Attached.Invoke(this);
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
        chainCount = -1;

        Detached.Invoke(this);
    }

    private void OnJointBreak2D(Joint2D joint)
    {
        JointBroken.Invoke(this);
    }

    private void OnAttachedDebrisBroken(Debris attachedDebris)
    {
        JointBroken.Invoke(this);
    }

    public PlayerInput GetPlayerShip()
    {
        if(chainCount < 0)
        {
            return null;
        }
        else
        {
            PlayerInput pInput = null;
            bool foundPlayer = false;
            Debris currentDebris = this;
            while(foundPlayer == false)
            {
                Debris newDebris = currentDebris.attachedJoint.connectedBody.GetComponent<Debris>();

                if(newDebris != null)
                {
                    currentDebris = newDebris;
                    continue;
                }
                else
                {
                    pInput = currentDebris.attachedJoint.connectedBody.GetComponent<PlayerInput>();
                    if (pInput == null)
                    {
                        Debug.LogError("Could not find the player");
                        break;
                    }
                    
                    break;
                }
            }

            return pInput;
        }
    }

    public class DebrisHitEvent : UnityEvent<Debris, Debris, Vector2> { }

    public class DebrisEvent : UnityEvent<Debris> { }
}
