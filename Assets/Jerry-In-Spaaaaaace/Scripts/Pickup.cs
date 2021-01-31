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

    private PlayerInput playerRef;
    private Collider2D myCollider;

    private Vector3 startingPos;
    private float lerpCounter = 0f;
    private float distanceToLerp;


    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        playerRef = FindObjectOfType<PlayerInput>();
        myCollider = gameObject.GetComponent<Collider2D>();

        switch (type)
        {
            case Type.Fuel:
                spriteR.sprite = fuelSprite;
                break;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            PickUpAction(MagnitudeDefined);
        }
        else if (collision.collider.CompareTag("Debris"))
        {
            if (collision.gameObject.GetComponent<Debris>().chainCount > 0)
            {
                PickUpAction(MagnitudeDefined);
            }
        }
    }

    void PickUpAction(int Magnitude){
        switch (type){
        case Type.Fuel:
                playerRef.Fuel.AddFuel(Magnitude);
                break;
        default:
            Debug.Log(@"I don't have an coded attached"+ type);
            break;
        }

        Destroy(gameObject);

        myCollider.enabled = false;

        startingPos = gameObject.transform.position;
        distanceToLerp = startingPos.magnitude - playerRef.transform.position.magnitude;

        StartCoroutine(LerpToPlayer());

    }

    private IEnumerator LerpToPlayer()
    {
        while (true)
        {
            if (gameObject.transform.position != playerRef.transform.position)
            {
                lerpCounter++;
                gameObject.transform.position = Vector3.Lerp(startingPos, playerRef.transform.position, lerpCounter/distanceToLerp);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                Destroy(gameObject);
                StopAllCoroutines();
            }
        }
    }
}
