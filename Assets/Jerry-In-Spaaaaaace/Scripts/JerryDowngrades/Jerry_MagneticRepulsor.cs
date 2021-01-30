using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Jerry_MagneticRepulsor : Jerry_RandomTimeEffect
{

    private List<Debris> debrisList = new List<Debris>();


    protected override void SetDefaults()
    {
        minWait = 35;
        maxWait = 115;
    }

    public override void triggerBehaviour()
    {

        debrisList = playerRef.GetConnectedDebris().Values.ToList<Debris>();

        foreach (Debris debris in debrisList)
        {
            GameObject goRef = debris.gameObject;

            debris.Detach();
            debris.JointBroken.Invoke(debris);

            Vector2 newForce = (goRef.transform.position - playerRef.gameObject.transform.position).normalized;
            goRef.GetComponent<Rigidbody2D>().AddForce(newForce * 50f);
        }
        Debug.Log("Jerry says it's boom time now");
    }
}
