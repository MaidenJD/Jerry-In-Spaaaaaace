using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class DebrisRandomiser : MonoBehaviour
{
    //This script randomly changes the visual and size of the debris
        
    private SpriteRenderer spriteR;
    public SpriteChance[] spriteChance;

    // Start is called before the first frame update
    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = getRandomSprite();

        float scaleSize = Random.Range(0.1f, 0.3f);
        if (Random.Range(1, 10) > 9)
        {
            scaleSize = scaleSize * 2;
        }

        gameObject.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        gameObject.transform.localScale = new Vector3(scaleSize, scaleSize, scaleSize);
    }


    private Sprite getRandomSprite()
    {
        int randWeight;
        int totalRange = 0;
        int currRange = 0;

        foreach (SpriteChance n in spriteChance)
        {
            totalRange = totalRange + n.weighting;
        }

        randWeight = Random.Range(0, totalRange);


        for (int i = 0; i < spriteChance.Length; i++)
        {            
            currRange = currRange + spriteChance[i].weighting;
            if (currRange > randWeight)
            {
                return spriteChance[i].sprite;
            }
        }

        //error catching
        Debug.LogError("Failed to locate RandWeight in DebrisRandomiser");
        return spriteChance[0].sprite;

    }
}

[System.Serializable]
    public class SpriteChance
{
    public Sprite sprite;
    public int weighting;
}