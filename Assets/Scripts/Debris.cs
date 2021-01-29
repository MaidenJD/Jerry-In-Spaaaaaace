using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Debris : MonoBehaviour
{
    public DebrisEvent CollisionHit = new DebrisEvent();

    public Rigidbody2D rb { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public void Attach(PlayerInput player)
    {

    }

    public class DebrisEvent : UnityEvent<Debris, Rigidbody2D> { }
}
