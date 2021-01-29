using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInitialImpulse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float customThrust = 1.0f;
        Vector2 randomDirection = new Vector3(Random.value, Random.value);
        gameObject.GetComponent<Rigidbody2D>().AddForce(randomDirection * customThrust);
    }
}
