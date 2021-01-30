using UnityEngine;
using UnityEngine.UI;

public class HUDComponent : MonoBehaviour
{
    public Slider FuelGauge { get; private set; }

    void Awake()
    {
        FuelGauge = transform.Find("Fuel Gauge").GetComponent<Slider>();
    }
}
