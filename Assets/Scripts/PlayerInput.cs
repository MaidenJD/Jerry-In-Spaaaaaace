using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float forceAmount = 1f;
    [SerializeField]
    private float torqueAmount = 1f;
    public Rigidbody2D rb { get; private set; }

    //private List<Debris> attachedDebris = new List<Debris>();
    private Dictionary<int, Debris> connectedDebris = new Dictionary<int, Debris>();

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector2 force = (transform.up * v) + (transform.right * h);
        force *= forceAmount;

        rb.AddForce(force);

        float r = Input.GetAxis("Rotate");

        rb.AddTorque(-r * torqueAmount);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            DetachAllDebris();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = 0; i < collision.contactCount; i++)
        {
            if(collision.contacts[i].collider.gameObject.CompareTag("Debris"))
            {
                var newDebris = collision.contacts[i].collider.gameObject.GetComponent<Debris>();
                if (!connectedDebris.ContainsKey(newDebris.GetInstanceID()))
                {
                    AttachDebrisToPlayer(newDebris, collision.contacts[i].point);
                    break;
                }
            }
        }
    }

    void AttachDebrisToPlayer(Debris newDebris, Vector2 hitPoint)
    {
        newDebris.Attach(rb, hitPoint);
        connectedDebris.Add(newDebris.GetInstanceID(), newDebris);

        newDebris.CollisionHit.AddListener(OnDebrisCollision);
    }

    void AttachDebrisToDebris(Debris attachedDebris, Debris hitDebris, Vector2 hitPoint)
    {
        //Tell the already hit debris to create a joint to the already attached debris
        hitDebris.Attach(attachedDebris.rb, hitPoint);
        //Add new Debris to list
        connectedDebris.Add(hitDebris.GetInstanceID(), hitDebris);

        hitDebris.CollisionHit.AddListener(OnDebrisCollision);

    }

    private void DetachAllDebris()
    {
        foreach(KeyValuePair<int, Debris> element in connectedDebris)
        {
            element.Value.Detach();
            element.Value.CollisionHit.RemoveListener(OnDebrisCollision);
        }

        connectedDebris.Clear();
    }

    private void OnDebrisCollision(Debris attachedDebris, Debris hitDebris, Vector2 hitPoint)
    {
        if(!connectedDebris.ContainsKey(hitDebris.GetInstanceID()))
        {
            AttachDebrisToDebris(attachedDebris, hitDebris, hitPoint);
        }
    }
}
