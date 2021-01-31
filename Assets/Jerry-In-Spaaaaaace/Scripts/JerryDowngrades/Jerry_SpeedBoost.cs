using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jerry_SpeedBoost : Jerry_RandomTimeEffect
{
    public float forceMultiplier = 200f;

    public override void triggerBehaviour()
    {
        base.triggerBehaviour();

        Debug.Log("Jerry says it's nyoom time now");
        Vector2 newForce = rb.velocity.normalized * forceMultiplier;
        rb.AddForce(newForce);
    }

    public override void jerryMessageDisplay()
    {
        int dialogueSwitch = Random.Range(1, 5);
        if (dialogueSwitch < 3)
        {
            jerryRef.ShowJerryMessage("Looks like you could do with a boost!");
            forceMultiplier = 200f;
        }
        else if (dialogueSwitch < 5)
        {
            jerryRef.ShowJerryMessage("Nyoom time!");
            forceMultiplier = 200f;
        }
        else
        {
            jerryRef.ShowJerryMessage("So... You seem pretty smart. What does 'overclock' mean? Just - the button's so red and tempting...");
            forceMultiplier = 600f;
        }
    }



}
