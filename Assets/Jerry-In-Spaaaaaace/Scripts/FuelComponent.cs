using UnityEngine;
using UnityEngine.UI;
using Jerry.Utilities;
using UnityEngine.Events;

namespace Jerry
{
    namespace Components
    {
        public class FuelComponent : MonoBehaviour
        {
            [Header("Runtime")]
            [SerializeField]
            [ReadOnly]
            private float RemainingFuel = 0;

            [Header("Design")]
            [SerializeField]
            private float StartingFuel = 300f;
            
            [SerializeField]
            private float FuelCap      = 300f;

            public UnityEvent OnFuelDepleted = new UnityEvent();

            private Slider FuelGauge;
            private PlayerInput Ship;

            public AudioClip clip;

            private void Start()
            {
                RemainingFuel = StartingFuel;

                HUDComponent HUD = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDComponent>();
                FuelGauge = HUD.FuelGauge;
                FuelGauge.maxValue = FuelCap;
            }

            public float RequestFuel(float DesiredAmount)
            {
                float FuelUsed = Mathf.Min(DesiredAmount, RemainingFuel);

                RemainingFuel -= FuelUsed;
                FuelGauge.value = RemainingFuel;

                return FuelUsed;
            }

            public void AddFuel(float Amount)
            {
                RemainingFuel = Mathf.Min(RemainingFuel + Amount, FuelCap);

                if (clip != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(clip);
                }
            }

            public float GetNormalizedFuel()
            {
                return RemainingFuel / FuelCap;
            }
        }
    }
}
