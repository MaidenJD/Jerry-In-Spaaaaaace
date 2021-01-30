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

    public float ZoomSpeed = 100;

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
#if ENABLE_INPUT_SYSTEM
        float scroll = zoomAction.ReadValue<float>();
#else
        float scroll = Input.mouseScrollDelta.y;
#endif

        Vector3 Pos = followTrans.position;
        Pos.z = transform.position.z;

        if (Mathf.Abs(scroll) > float.Epsilon)
        {
            float ZoomOffset = scroll * ZoomSpeed;
            Pos.z += ZoomOffset;
            Pos.z = Mathf.Clamp(Pos.z, -100, -10);
        }

        transform.position = Pos;
    }
}
