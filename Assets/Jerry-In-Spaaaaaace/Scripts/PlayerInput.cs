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
    public List<ParticleSystem> ShipThrusters;

    /// <summary>
    /// Information about all of the Active thrusters attached to the ship
    /// </summary>
    private List<ThrusterInfo> AllThrusters;
    /// <summary>
    /// The indexes of all direction thrusters
    /// </summary>
    private List<int> DirectionalThrusters;
    /// <summary>
    /// The indexes of all Clockwise thrusters
    /// </summary>
    private List<int> ClockwiseThrusters;
    /// <summary>
    /// The indexes of all Anti-clockwise thrusters
    /// </summary>
    private List<int> AnticlockwiseThrusters; 
    
    
    public Rigidbody2D rb { get; private set; }

    private Dictionary<int, Debris> connectedDebris = new Dictionary<int, Debris>();

    private bool allowDebris = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Fuel = GetComponent<FuelComponent>();

        AssignThrusters();
    }

    public Dictionary<int, Debris> GetConnectedDebris()
    {
        return connectedDebris;
    }

    private void Update()
    {
        //Reset the thruster state
        //Keep track of the Thruster that are going to be activated
        bool[] ThrusterStates = new bool[AllThrusters.Count];

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
    	    PlayDirectionalThrusters(-localForce.normalized, ref ThrusterStates);
        }

        float r = Input.GetAxis("Rotate");
        r = -r * torqueAmount;

        DesiredFuel        = Mathf.Abs(r) * Time.deltaTime;
        FuelUsed           = Fuel.RequestFuel(DesiredFuel);

        if (FuelUsed > 0)
        {
            float NormalizedFuelUsed = FuelUsed / DesiredFuel;
            r *= NormalizedFuelUsed;

            rb.AddTorque(r);

            if (r > Mathf.Epsilon)
            {
                PlayClockwiseThrusters(ref ThrusterStates);
            }
            else if (r < -Mathf.Epsilon)
            {
                PlayAnticlockwiseThrusters(ref ThrusterStates);
            }
        }

        //Set the Thrusters state
        SetThrusterStates(ref ThrusterStates);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            DetachAllDebris();
        }
    }

    #region Thruster Methods
    private void AssignThrusters()
    {
        int thrusterCount = ShipThrusters.Count;
        AllThrusters = new List<ThrusterInfo>(thrusterCount);
        DirectionalThrusters = new List<int>(thrusterCount);

        ClockwiseThrusters = new List<int>();
        AnticlockwiseThrusters = new List<int>();

        //Loop through each ship thruster
        for (int i = 0; i < thrusterCount; i++)
        {
            Transform thrusterTrans = ShipThrusters[i].transform;
            //Save this thruster into all thruster array
            AllThrusters.Add(new ThrusterInfo(ShipThrusters[i], thrusterTrans.localRotation * Vector2.up));
            //Get the direction of the thruster and save it
            DirectionalThrusters.Add(i);
            //Stop each thruster
            AllThrusters[i].thruster.Stop();


            //Check if this thruster can be a rotational thruster
            Vector2 dirToThruster = thrusterTrans.localPosition;
            Vector2 thrusterDir = AllThrusters[i].Direction;
            float dotProd = Vector2.Dot(dirToThruster, thrusterDir);
            Vector3 crossProd = Vector3.Cross(dirToThruster, thrusterDir);
            //Debug.Log($"{AllThrusters[i].gameObject.name}: {dotProd}, cross: {crossProd}", AllThrusters[i].gameObject);
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

    private void AddThruster(ParticleSystem newThruster)
    {
        int thrusterIndex = AllThrusters.Count;
        AllThrusters.Add(new ThrusterInfo(newThruster, transform));

        DirectionalThrusters.Add(thrusterIndex);
        //Tell this thruster to stop
        newThruster.Stop();

        //Calculate if this is a rotational thruster
        Vector2 dirToThruster = transform.InverseTransformPoint(newThruster.transform.position);
        Vector2 thrusterDir = AllThrusters[thrusterIndex].Direction;
        float dotProd = Vector3.Dot(dirToThruster, thrusterDir);
        Vector3 crossProd = Vector3.Cross(dirToThruster, thrusterDir);

        if(Mathf.Abs(dotProd) < 0.2f)
        {
            if(crossProd.z < 0f)
            {
                ClockwiseThrusters.Add(thrusterIndex);
            }
            else
            {
                AnticlockwiseThrusters.Add(thrusterIndex);
            }
        }
    }

    private void RemoveThruster(ParticleSystem removeThruster)
    {
        //Find the index of thruster
        int thrusterInstanceID = removeThruster.GetInstanceID();
        int index = -1;
        for(int i = 0; i < AllThrusters.Count; i++)
        {
            if(AllThrusters[i].InstanceID == thrusterInstanceID)
            {
                index = i;
                break;
            }
        }

        if(index == -1)
        {
            Debug.LogError($"Failed to remove thruster {removeThruster.gameObject.name} because it doesn't exist in array", removeThruster.gameObject);
            return;
        }

        ClockwiseThrusters.Remove(index);
        AnticlockwiseThrusters.Remove(index);
        DirectionalThrusters.Remove(index);
        AllThrusters.RemoveAt(index);
    }

    void PlayDirectionalThrusters(Vector2 dir, ref bool[] ThrusterStates)
    {
        int thrusterCount = DirectionalThrusters.Count;
        for(int i = 0; i < thrusterCount; i++)
        {
            int index = DirectionalThrusters[i];
            float dot = Vector3.Dot(AllThrusters[index].Direction, dir);
            bool activeThruster = dot > 0.707f;

            if (!ThrusterStates[i])
            {
                ThrusterStates[i] = activeThruster;
            }
            
        }
    }

    void PlayClockwiseThrusters(ref bool[] ThrusterStates)
    {
        for(int i = 0; i < ClockwiseThrusters.Count; i++)
        {
            if(!ThrusterStates[ClockwiseThrusters[i]])
            {
                ThrusterStates[ClockwiseThrusters[i]] = true;
            }
        }
    }

    void PlayAnticlockwiseThrusters(ref bool[] ThrusterStates)
    {
        for(int i = 0; i < AnticlockwiseThrusters.Count; i++)
        {
            if(!ThrusterStates[AnticlockwiseThrusters[i]])
            {
                ThrusterStates[AnticlockwiseThrusters[i]] = true;
            }
        }
    }

    void SetThrusterStates(ref bool[] ThrusterStates)
    {
        for(int i = 0; i < ThrusterStates.Length; i++)
        {
            if(ThrusterStates[i] != AllThrusters[i].thruster.isPlaying)
            {
                if(ThrusterStates[i])
                {
                    AllThrusters[i].thruster.Play();
                }
                else
                {
                    AllThrusters[i].thruster.Stop();
                }
            }
        }
    }

    public struct ThrusterInfo
    {
        public int InstanceID;
        public ParticleSystem thruster;
        public Vector2 Direction;

        public ThrusterInfo(ParticleSystem ThrusterParticle, Vector2 ThrusterDirection)
        {
            InstanceID = ThrusterParticle.GetInstanceID();
            thruster = ThrusterParticle;
            Direction = ThrusterDirection;
        }

        public ThrusterInfo(ParticleSystem ThrusterParticle, Transform shipTrans)
        {
            InstanceID = ThrusterParticle.GetInstanceID();
            thruster = ThrusterParticle;
            Direction = shipTrans.InverseTransformDirection(ThrusterParticle.transform.up);
        }

        public override bool Equals(object obj)
        {
            if(obj is ThrusterInfo otherThruster)
            {
                return this.InstanceID == otherThruster.InstanceID;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public static bool operator ==(ThrusterInfo a, ThrusterInfo b)
        {
            return a.InstanceID == b.InstanceID;
        }

        public static bool operator !=(ThrusterInfo a, ThrusterInfo b)
        {
            return a.InstanceID != b.InstanceID;
        }

        public override int GetHashCode()
        {
            return this.InstanceID;
        }
    }
    #endregion

    #region Debris
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

        //Check if there is a thruster on this debris
        if(newDebris.Thruster)
        {
            AddThruster(newDebris.Thruster);
        }
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

            if(element.Value.Thruster != null)
            {
                RemoveThruster(element.Value.Thruster);
            }
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

        if(debris.Thruster != null)
        {
            RemoveThruster(debris.Thruster);
        }

        connectedDebris.Remove(debris.GetInstanceID());
    }
    #endregion
}
