using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum Type{
        Fuel
    }
    public Type type= 0;
    private SpriteRenderer spriteR;
    public Sprite fuelSprite;
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
        switch (type){
        case Type.Fuel:
            spriteR = gameObject.GetComponent<SpriteRenderer>();
                spriteR.sprite = fuelSprite;
            //code here
            //Debug.Log(@"adds fuel: "+ Magnitude);    
                break;
        default:
            Debug.Log(@"I don't have an coded attached"+ type);
            break;
        }
        
    }
}
