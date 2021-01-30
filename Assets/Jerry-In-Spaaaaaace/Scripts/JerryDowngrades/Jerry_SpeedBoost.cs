using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jerry_SpeedBoost : Jerry_RandomTimeEffect
{
    public float forceMultiplier = 200f;

    public override void triggerBehaviour()
    {
        Debug.Log("Jerry says it's nyoom time now");
        Vector2 newForce = rb.velocity.normalized * forceMultiplier;
        rb.AddForce(newForce);
    }

}
