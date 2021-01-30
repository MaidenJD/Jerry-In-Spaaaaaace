using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private float forceAmount = 1f;
    [SerializeField]
    private float torqueAmount = 1f;

    [Header("Fuel Settings")]
    [SerializeField]
    private float StartingFuelAmount = 1000f;
    public bool bNeedFuel = true;
    public Rigidbody2D rb { get; private set; }

    private Dictionary<int, Debris> connectedDebris = new Dictionary<int, Debris>();

    private bool allowDebris = true;

    private float fuel;
    /// <summary>
    /// The Amount of Fuel left on the ship
    /// </summary>
    public float Fuel => fuel;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuel = StartingFuelAmount;
    }

    private void Update()
    {
        if (fuel > 0f || !bNeedFuel)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector2 force = (transform.up * v) + (transform.right * h);
            force *= forceAmount;

            rb.AddForce(force);

            if(bNeedFuel)
                fuel -= force.magnitude * Time.deltaTime;
        }

        if (fuel > 0f || !bNeedFuel)
        {
            float r = Input.GetAxis("Rotate");
            r = -r * torqueAmount;
            rb.AddTorque(r);

            if(bNeedFuel)
                fuel -= Mathf.Abs(r) * Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            DetachAllDebris();
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

#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerInput))]
    public class Inspector : Editor
    {
        private PlayerInput script;
        

        private void OnEnable()
        {
            script = (PlayerInput)target;    
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if(Application.isPlaying)
                GUILayout.Label($"Fuel: {script.fuel}");
        }
    }
#endif
}
