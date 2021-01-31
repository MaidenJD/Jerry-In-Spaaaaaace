using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Jerry_MagneticRepulsor : Jerry_RandomTimeEffect
{

    private List<Debris> debrisList = new List<Debris>();
    public float forceMultiplier;


    protected override void SetDefaults()
    {
        minWait = 35;
        maxWait = 115;
    }

    public override void triggerBehaviour()
    {
        base.triggerBehaviour();

        debrisList = playerRef.GetConnectedDebris().Values.ToList<Debris>();

        foreach (Debris debris in debrisList)
        {
            GameObject goRef = debris.gameObject;

            debris.Detach();
            debris.JointBroken.Invoke(debris);

            Vector2 newForce = (goRef.transform.position - playerRef.gameObject.transform.position).normalized;
            goRef.GetComponent<Rigidbody2D>().AddForce(newForce * forceMultiplier);
        }
        Debug.Log("Jerry says it's boom time now");
    }

    public override void jerryMessageDisplay()
    {        
        switch (Random.Range(1, 3))
        {
            case 1:
                jerryRef.ShowJerryMessage("Hey there best friend! I detected some stuff stuck to your stuff, so I thought I'd boot up the ol' repulsor. You're welcome!");
                forceMultiplier = 50f;
                break;
            case 2:
                jerryRef.ShowJerryMessage("Hey, check this out... Computer! Active the Magnetic Repulsor! Maximum yeet!");
                forceMultiplier = 200f;
                break;
            default:
                jerryRef.ShowJerryMessage("Hi buddy! Seems you've got some excess baggage there and your ship could lose a few pounds. How about I use that replusor I was talking about earlier?");
                forceMultiplier = 50f;
                break;
        }
    }
}
