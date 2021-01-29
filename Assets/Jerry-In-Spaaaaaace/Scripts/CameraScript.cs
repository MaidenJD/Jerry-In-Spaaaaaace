using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform followTrans;
    public Camera gameCamera;
    // Start is called before the first frame update
    void Start()
    {
        gameCamera = GetComponent<Camera>();

        if (followTrans == null)
            enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = followTrans.position + (Vector3.forward * -10f);
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > Mathf.Epsilon)
        {
            gameCamera.orthographicSize += -scroll * gameCamera.orthographicSize * 0.1f; 
        }
    }
}
