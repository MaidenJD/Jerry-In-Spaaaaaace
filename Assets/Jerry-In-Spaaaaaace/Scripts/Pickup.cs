using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public int MagnitudeDefined = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PickUpAction(MagnitudeDefined);
            Destroy(gameObject);
        }
    }

    void PickUpAction(int Magnitude){
        //code here
        //Debug.Log(@"I've been picked up");
    }
}
