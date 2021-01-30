using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSpriteSelector : MonoBehaviour
{
    private Objective objRef;
    private SpriteRenderer spriteR;
    public Sprite carSprite;
    public Sprite sputnikSprite;

    // Start is called before the first frame update
    void Start()
    {
        objRef = gameObject.GetComponent<Objective>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();

        switch (objRef.Type)
        {
            case ObjectiveType.ElonMuskCar:
                spriteR.sprite = carSprite;
                break;
            case ObjectiveType.Sputnik:
                spriteR.sprite = sputnikSprite;
                break;
            default:
                Debug.Log(@"No Objective sprite for " + objRef.Type);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
