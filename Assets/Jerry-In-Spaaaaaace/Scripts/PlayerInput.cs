using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using Jerry;
using Jerry.Components;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float forceAmount = 1f;
    [SerializeField]
    private float torqueAmount = 1f;

    [HideInInspector]
    public FuelComponent Fuel { get; private set; }


    [Header("Thrusters")]
    public ParticleSystem[] AllThrusters;

    private List<int> DirectionalThrusters;
    private List<int> ClockwiseThrusters;
    private List<int> AnticlockwiseThrusters; 
    private Vector3[] DirectionalThrustersDirections;
    
    [SerializeField] private bool[] CurrentThrusterState;
    public Rigidbody2D rb { get; private set; }

    private Dictionary<int, Debris> connectedDebris = new Dictionary<int, Debris>();

    private bool allowDebris = true;

    private void Start()
    {
        rb   = GetComponent<Rigidbody2D>();
        Fuel = GetComponent<FuelComponent>();

        AssignThrusters();
    }

    private void AssignThrusters()
    {
        int thrusterCount = AllThrusters.Length;
        DirectionalThrusters = new List<int>(thrusterCount);
        DirectionalThrustersDirections = new Vector3[thrusterCount];
        ClockwiseThrusters = new List<int>();
        AnticlockwiseThrusters = new List<int>();
        CurrentThrusterState = new bool[thrusterCount];
        
        for (int i = 0; i < AllThrusters.Length; i++)
        {
            //Get the direction of the thruster and save it
            DirectionalThrustersDirections[i] = AllThrusters[i].transform.localRotation * Vector3.up;
            DirectionalThrusters.Add(i);
            //Stop each thruster
            AllThrusters[i].Stop();


            //Check if this thruster can be a rotational thruster
            Vector3 dirToThruster = AllThrusters[i].transform.localPosition;
            float dotProd = Vector3.Dot(dirToThruster, DirectionalThrustersDirections[i]);
            Vector3 crossProd = Vector3.Cross(dirToThruster, DirectionalThrustersDirections[i]);
            Debug.Log($"{AllThrusters[i].gameObject.name}: {dotProd}, cross: {crossProd}", AllThrusters[i].gameObject);
            if (Mathf.Abs(dotProd) < 0.2f)
            {
                if (crossProd.z < 0f)
                {
                    ClockwiseThrusters.Add(i);
                }
                else
                {
                    AnticlockwiseThrusters.Add(i);
                }
            }
        }
    }

    private void Update()
    {
        //Reset the thruster state
        for(int i = 0; i < CurrentThrusterState.Length; i++)
        {
            CurrentThrusterState[i] = false;
        }

        //if (fuel > 0f || !bNeedFuel)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector2 force = (transform.up * v) + (transform.right * h);
            force *= forceAmount;

            float DesiredFuel        = force.magnitude * Time.deltaTime;
            float FuelUsed           = Fuel.RequestFuel(DesiredFuel);

            if (FuelUsed > 0)
            {
                float NormalizedFuelUsed = FuelUsed / DesiredFuel;
                force *= NormalizedFuelUsed;

                rb.AddForce(force);
				
				//Play Particle Effects
	            Vector2 localForce = new Vector2(h, v);
    	        PlayDirectionalThrusters(-localForce.normalized);
            }

        }

        {
            float r = Input.GetAxis("Rotate");
            r = -r * torqueAmount;

            if(r > Mathf.Epsilon)
            {
                PlayClockwiseThrusters();
            }
            else if(r < -Mathf.Epsilon)
            {
                PlayAnticlockwiseThrusters();
            }

            //if (bNeedFuel)
            float DesiredFuel        = Mathf.Abs(r) * Time.deltaTime;
            float FuelUsed           = Fuel.RequestFuel(DesiredFuel);

            if (FuelUsed > 0)
            {
                float NormalizedFuelUsed = FuelUsed / DesiredFuel;
                r *= NormalizedFuelUsed;

                rb.AddTorque(r);
            }
        }

        //Set the Thrusters state
        SetThrusterStates();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            DetachAllDebris();
        }
    }

    void PlayDirectionalThrusters(Vector2 dir)
    {
        int thrusterCount = DirectionalThrusters.Count;
        for(int i = 0; i < thrusterCount; i++)
        {
            float dot = Vector3.Dot(DirectionalThrustersDirections[i], dir);
            bool activeThruster = dot > 0.707f;

            if (!CurrentThrusterState[i])
            {
                CurrentThrusterState[i] = activeThruster;
            }
            
        }
    }

    void PlayClockwiseThrusters()
    {
        for(int i = 0; i < ClockwiseThrusters.Count; i++)
        {
            if(!CurrentThrusterState[ClockwiseThrusters[i]])
            {
                CurrentThrusterState[ClockwiseThrusters[i]] = true;
            }
        }
    }

    void PlayAnticlockwiseThrusters()
    {
        for(int i = 0; i < AnticlockwiseThrusters.Count; i++)
        {
            if(!CurrentThrusterState[AnticlockwiseThrusters[i]])
            {
                CurrentThrusterState[AnticlockwiseThrusters[i]] = true;
            }
        }
    }

    void SetThrusterStates()
    {
        for(int i = 0; i < CurrentThrusterState.Length; i++)
        {
            if(CurrentThrusterState[i] != AllThrusters[i].isPlaying)
            {
                if(CurrentThrusterState[i])
                {
                    AllThrusters[i].Play();
                }
                else
                {
                    AllThrusters[i].Stop();
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (allowDebris)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.contacts[i].collider.gameObject.CompareTag("Debris"))
                {
                    var newDebris = collision.contacts[i].collider.gameObject.GetComponent<Debris>();
                    if (!connectedDebris.ContainsKey(newDebris.GetInstanceID()))
                    {
                        AttachDebrisToPlayer(newDebris, collision.contacts[i].point);
                        break;
                    }
                }
            }
        }
    }

    void AttachDebrisToPlayer(Debris newDebris, Vector2 hitPoint)
    {
        newDebris.Attach(rb, hitPoint);
        connectedDebris.Add(newDebris.GetInstanceID(), newDebris);

        newDebris.CollisionHit.AddListener(OnDebrisCollision);
        newDebris.JointBroken.AddListener(OnDebrisJointBroken);
    }

    void AttachDebrisToDebris(Debris attachedDebris, Debris hitDebris, Vector2 hitPoint)
    {
        //Tell the already hit debris to create a joint to the already attached debris
        hitDebris.Attach(attachedDebris.rb, hitPoint);
        //Add new Debris to list
        connectedDebris.Add(hitDebris.GetInstanceID(), hitDebris);

        hitDebris.CollisionHit.AddListener(OnDebrisCollision);
        hitDebris.JointBroken.AddListener(OnDebrisJointBroken);
    }

    private void DetachAllDebris()
    {
        foreach(KeyValuePair<int, Debris> element in connectedDebris)
        {
            element.Value.Detach();
            element.Value.CollisionHit.RemoveListener(OnDebrisCollision);
            element.Value.JointBroken.RemoveListener(OnDebrisJointBroken);
        }

        connectedDebris.Clear();

        allowDebris = false;
        Invoke(nameof(EnableDebris), 1f);
    }

    private void EnableDebris()
    {
        allowDebris = true;
    }

    private void OnDebrisCollision(Debris attachedDebris, Debris hitDebris, Vector2 hitPoint)
    {
        if(!connectedDebris.ContainsKey(hitDebris.GetInstanceID()))
        {
            AttachDebrisToDebris(attachedDebris, hitDebris, hitPoint);
        }
    }

    private void OnDebrisJointBroken(Debris debris)
    {
        debris.Detach();
        debris.CollisionHit.RemoveListener(OnDebrisCollision);
        debris.JointBroken.RemoveListener(OnDebrisJointBroken);

        connectedDebris.Remove(debris.GetInstanceID());
    }
}
