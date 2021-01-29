using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float forceAmount;
    public Rigidbody2D rb { get; private set; }

    private List<Debris> attachedDebris = new List<Debris>();

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

        if(Input.GetKeyDown(KeyCode.Space))
        {
            DetachAllDebris();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Debris"))
        {
            var newDebris = collision.gameObject.GetComponent<Debris>();
            if(!attachedDebris.Contains(newDebris))
            {
                AttachDebrisToPlayer(newDebris);
            }
        }
    }

    void AttachDebrisToPlayer(Debris newDebris)
    {
        newDebris.Attach(rb);
        attachedDebris.Add(newDebris);

        newDebris.CollisionHit.AddListener(OnDebrisCollision);
    }

    void AttachDebrisToDebris(Debris attachedDebris, Debris hitDebris)
    {
        //Tell the already hit debris to create a joint to the already attached debris
        hitDebris.Attach(attachedDebris.rb);
        //Add new Debris to list
        this.attachedDebris.Add(attachedDebris);

        hitDebris.CollisionHit.AddListener(OnDebrisCollision);

    }

    private void DetachAllDebris()
    {
        for(int i = 0; i < attachedDebris.Count; i++)
        {
            attachedDebris[i].Detach();
            attachedDebris[i].CollisionHit.RemoveListener(OnDebrisCollision);
        }

        attachedDebris.Clear();
    }

    private void OnDebrisCollision(Debris attachedDebris, Debris hitDebris)
    {
        if(!this.attachedDebris.Contains(hitDebris))
        {
            AttachDebrisToDebris(attachedDebris, hitDebris);
        }
    }
}
