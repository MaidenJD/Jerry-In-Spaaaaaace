using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float forceAmount;
    private Rigidbody2D rb;

    private List<Debris> attachedDebris;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Debris"))
        {
            Rigidbody2D otherRB = collision.collider.GetComponent<Rigidbody2D>();
            var joint = otherRB.gameObject.AddComponent<FixedJoint2D>();
            joint.connectedBody = rb;


        }
    }
}
