using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    public GameObject Sprite;
    public float RotationSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sprite.transform.Rotate(new Vector3(0, 0, Time.deltaTime * RotationSpeed));
    }
}
