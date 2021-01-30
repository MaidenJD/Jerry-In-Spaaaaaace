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

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerRef = gameObject.GetComponent<PlayerInput>();
        genNextTime();
        StartCoroutine(WaitSecond());
    }

    protected virtual void SetDefaults()
    {

    }

    private IEnumerator WaitSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timer++;
            if (timer >= nextTime)
            {
                timer = 0;
                genNextTime();
                triggerBehaviour();
            }
        }
    }

    private void genNextTime()
    {
        nextTime = Random.Range(minWait, maxWait);
    }

    public virtual void triggerBehaviour()
    {
    }


}
