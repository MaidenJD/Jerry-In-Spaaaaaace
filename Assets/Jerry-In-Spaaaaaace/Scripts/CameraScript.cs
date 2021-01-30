using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class CameraScript : MonoBehaviour
{
    public Transform followTrans;
    public Camera gameCamera;

#if ENABLE_INPUT_SYSTEM
    private InputAction zoomAction;
#endif
    // Start is called before the first frame update
    void Start()
    {
        gameCamera = GetComponent<Camera>();

        if (followTrans == null)
            enabled = false;
    }
#if ENABLE_INPUT_SYSTEM
    private void OnEnable()
    {
        zoomAction = new SpaceControls().Gameplay.Zoom;
        zoomAction.Enable();
    }

    private void OnDisable()
    {
        zoomAction.Enable();
        zoomAction = null;
    }
#endif

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = followTrans.position + (Vector3.forward * -10f);
#if ENABLE_INPUT_SYSTEM
        float scroll = zoomAction.ReadValue<float>();
#else
        float scroll = Input.mouseScrollDelta.y;
#endif
        if (Mathf.Abs(scroll) > Mathf.Epsilon)
        {
            gameCamera.orthographicSize += -scroll * gameCamera.orthographicSize * 0.1f; 
        }
    }
}
