using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInitialImpulse : MonoBehaviour
{

    Renderer m_Renderer;
    Rigidbody2D rb;
    public float customThrust = 1;
    public Vector2 overrideDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.Sleep();
        m_Renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (m_Renderer.isVisible && rb.IsSleeping())
        {
            rb.WakeUp();
            randomForceDirection();
        }
    }

    private void randomForceDirection()
    {        
        Vector2 randomDirection = new Vector3(Random.value, Random.value);
        if (overrideDirection.magnitude > 0) randomDirection = overrideDirection;
        customThrust = Random.Range((customThrust * 0.9f), (customThrust * 1.1f));
        gameObject.GetComponent<Rigidbody2D>().AddForce(randomDirection * customThrust);
    }
}


