using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform followTrans;
    // Start is called before the first frame update
    void Start()
    {
        if (followTrans == null)
            enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = followTrans.position + (Vector3.forward * -10f);
    }
}
