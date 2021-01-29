using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisRandomiser : MonoBehaviour
{
    //This script randomly changes the visual and size of the debris

    private SpriteRenderer spriteR;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = sprites[Random.Range(0, sprites.Length)];

        float scaleSize = Random.Range(0.1f, 0.3f);
        if (Random.Range(1,10)>9)
        {
            scaleSize = scaleSize * 2;
        }

        gameObject.transform.eulerAngles = new Vector3 (0, 0, Random.Range(0, 360));
        gameObject.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
    }
}
