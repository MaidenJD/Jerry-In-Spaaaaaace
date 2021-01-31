using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuFloater : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        col.transform.position = new Vector3(-1.83000004f, Random.Range(0.4f, -2.5f), 0.550000012f);
        col.gameObject.GetComponent<DebrisRandomiser>().changeToNewSprite();
    }
}
