using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jerry_RandomTimeEffect : MonoBehaviour
{    
    [SerializeField]
    protected int timer;
    public int minWait = 5;
    public int maxWait = 55;
    [SerializeField]
    protected int nextTime;
    protected PlayerInput playerRef;
    protected Rigidbody2D rb;
    protected JerryMessage jerryRef;

    protected bool pauseTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        jerryRef = FindObjectOfType<JerryMessage>();
        playerRef = gameObject.GetComponent<PlayerInput>();

        jerryRef.messageGone.AddListener(UnpauseTimer);
        jerryRef.messageStart.AddListener(PauseTimer);


        genNextTime();
        StartCoroutine(WaitSecond());
    }

    void UnpauseTimer()
    {
        pauseTimer = false;
    }

    void PauseTimer()
    {
        pauseTimer = true;
    }


    protected virtual void SetDefaults()
    {

    }

    private IEnumerator WaitSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (!pauseTimer) timer++;

            if (timer >= nextTime)
            {
                timer = 0;
                genNextTime();

                StopCoroutine(WaitSecond());
                
                jerryRef.messageIntroDone.AddListener(triggerBehaviour);
                jerryMessageDisplay();
            }
        }
    }

    private void genNextTime()
    {
        nextTime = Random.Range(minWait, maxWait);
    }

    public virtual void jerryMessageDisplay()
    {

    }

    public virtual void triggerBehaviour()
    {
        jerryRef.messageIntroDone.RemoveListener(triggerBehaviour);
    }

}
