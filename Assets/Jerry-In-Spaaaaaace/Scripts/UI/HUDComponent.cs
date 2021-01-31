using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class HUDComponent : MonoBehaviour
{
    [HideInInspector]
    public Slider FuelGauge { get; private set; }

    [HideInInspector]
    public Image OnScreenObjective { get; private set; }

    [HideInInspector]
    public Image OffScreenObjective { get; private set; }

    [HideInInspector]
    private GameObject Objective;
    private float ObjectiveTimeDilation = 20;

    JerryMessage MessageFromJerry;

    [HideInInspector]
    private PlayerInput Ship;

    void Awake()
    {
        FuelGauge = transform.Find("Fuel Gauge").GetComponent<Slider>();

        OnScreenObjective = transform.Find("Objective Marker").Find("On Screen Objective").GetComponent<Image>();
        OffScreenObjective = transform.Find("Objective Marker").Find("Off Screen Objective").GetComponent<Image>();

        Ship = GameObject.FindObjectOfType<PlayerInput>();

        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    void Update()
    {
        OnScreenObjective.enabled  = false;
        OffScreenObjective.enabled = false;

        if (Objective != null)
        {
            Rect   ScreenRect   = Camera.main.pixelRect;
            Bounds ScreenBounds = new Bounds(ScreenRect.center, ScreenRect.size);
            ScreenBounds.Expand(new Vector3(0, 0, float.MaxValue));

            Vector3 ObjectiveScreenPos = Camera.main.WorldToScreenPoint(Objective.transform.position);

            bool bOnScreen = ScreenBounds.Contains(ObjectiveScreenPos);

            OnScreenObjective.enabled  =  bOnScreen;
            OffScreenObjective.enabled = !bOnScreen;

            if (bOnScreen)
            {
                OnScreenObjective.rectTransform.position = Camera.main.ScreenToWorldPoint(ObjectiveScreenPos);
            }
            else
            {
                Vector2 HalfSize = OffScreenObjective.GetPixelAdjustedRect().size / 2;

                Vector3 Pos = Camera.main.ScreenToWorldPoint(
                    new Vector3(
                        Mathf.Clamp(ObjectiveScreenPos.x, HalfSize.x, ScreenBounds.size.x - HalfSize.x),
                        Mathf.Clamp(ObjectiveScreenPos.y, HalfSize.y, ScreenBounds.size.y - HalfSize.y),
                        10));

                OffScreenObjective.rectTransform.position = Pos;

                Vector3 ShipScreenPos = Camera.main.WorldToScreenPoint(Ship.transform.position);

                float Angle = Mathf.Atan2(ShipScreenPos.y - ObjectiveScreenPos.y, ShipScreenPos.x - ObjectiveScreenPos.x) * Mathf.Rad2Deg + 90;
                Quaternion Rot = Quaternion.Euler(Vector3.forward * Angle);
                OffScreenObjective.rectTransform.rotation = Quaternion.Slerp(OffScreenObjective.rectTransform.rotation, Rot, Time.deltaTime * ObjectiveTimeDilation);
            }
        }
    }

    public void ClearObjective()
    {
        Objective = null;
    }

    public void SetObjective(GameObject NewObjective)
    {
        Objective = NewObjective;
    }
}
