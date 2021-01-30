using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    public GameObject Sprite;
    public float RotationSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        Sprite.transform.Rotate(new Vector3(0, 0, Time.deltaTime * RotationSpeed));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var objectiveHit = collision.gameObject.GetComponent<Objective>() ?? null;

        if(objectiveHit != null)
        {
            //win
        }
    }
}