using UnityEngine;
using UnityEngine.UI;

public class HUDComponent : MonoBehaviour
{
    [HideInInspector]
    public Slider FuelGauge { get; private set; }

    [HideInInspector]
    public Image OnScreenObjective { get; private set; }

    [HideInInspector]
    public Image OffScreenObjective { get; private set; }

    [HideInInspector]
    private Objective Objective;

    void Awake()
    {
        FuelGauge = transform.Find("Fuel Gauge").GetComponent<Slider>();

        OnScreenObjective = transform.Find("Objective Marker").Find("On Screen Objective").GetComponent<Image>();
        OffScreenObjective = transform.Find("Objective Marker").Find("Off Screen Objective").GetComponent<Image>();

        Objective = GameObject.FindObjectOfType<Objective>();
    }

    void Update()
    {
        OnScreenObjective.enabled  = false;
        OffScreenObjective.enabled = false;

        if (Objective != null)
        {
            Rect   ScreenRect   = Camera.main.pixelRect;
            Bounds ScreenBounds = new Bounds(ScreenRect.center, ScreenRect.size);
            ScreenBounds.Expand(new Vector3(0, 0, float.PositiveInfinity));

            Vector3 ObjectiveScreenPos = Camera.main.WorldToScreenPoint(Objective.transform.position);

            bool bOnScreen = ScreenBounds.Contains(ObjectiveScreenPos);

            OnScreenObjective.enabled  =  bOnScreen;
            OffScreenObjective.enabled = !bOnScreen;

            if (bOnScreen)
            {
                OnScreenObjective.rectTransform.position = ObjectiveScreenPos;
            }
            else
            {
                OffScreenObjective.rectTransform.position = Vector3.Min(Vector3.Max(ObjectiveScreenPos, Vector3.zero), ScreenRect.size);
            }
        }
    }
}
