using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WallScript : MonoBehaviour
{
    public float minimum = 1.0F;
    public float maximum =  2.0F;
    [Tooltip("Are You brave? are you?")]
    public bool guts = false;
    void Start()
    {
        //allows for a random size for the walls so they don't looks so bland
        //does not affect the collison so is only astetic
        var spriteShapeController = GetComponent<SpriteShapeController>();
        var spline = spriteShapeController.spline;
        for (int i = 0; i < spline.GetPointCount(); ++i)
        {
            spline.SetHeight(i,Random.Range(minimum, maximum));
        }
    }
        
        static float t =0.0f;
        static float tempt = 0.0f;
    // Update is called once per frame
    void Update()
    {
        //I am not proud of this code (pretty sure I could achive the same results with a sin wave or something)
        //But I am proud of it's results
        if (guts == true){
            var spriteShapeController = GetComponent<SpriteShapeController>();
            var spline = spriteShapeController.spline;
            for (int i = 0; i < spline.GetPointCount(); ++i)
            {
                tempt=  (t% 2) + (0.5f*(i%4));
                if (tempt > 1.0f)
                    tempt = 2.0f - tempt;
                if (tempt <0)
                    tempt *= -1.0f;
                if (tempt > 1.0f)
                    tempt = 2.0f - tempt;
                float pos = spline.GetHeight(i);
                pos = (Mathf.Lerp(minimum, maximum,tempt));
                spline.SetHeight(i,pos);
            }
            t += 0.5f * Time.deltaTime;
            if (t > 2.0f)
            t = 0.0f;
        }
    }
}
