using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JerryMessage : MonoBehaviour
{
    private Animation Animations;
    private TMPro.TMP_Text Text;
    private float CloseDelay;
    
    private bool bCanShowMessage = true;

    void Start()
    {
        Animations = GetComponent<Animation>();
        Text = GetComponentInChildren<TMPro.TMP_Text>();

        ShowJerryMessage("Sup Mother Fucker, I've had enough of your shit. I deserve Employee of the Month, DESERVE IT. I hear you joking about me back at the station. Just watch your back my friend, never know what can happen in space...");
    }

    public void ShowJerryMessage(string Message, float CloseDelay = 10f)
    {
        if (bCanShowMessage)
        {
            bCanShowMessage = false;

            Text.text = Message;
            Animations.Play("Incoming Message");
            this.CloseDelay = CloseDelay;
        }
    }

    public void MessageIntroFinished()
    {
        StartCoroutine(DelayedMessageClosure(CloseDelay));
    }

    IEnumerator DelayedMessageClosure(float CloseDelay)
    {
        yield return new WaitForSeconds(CloseDelay);
        Animations.Play("Incoming Message Reverse");
    }

    public void MessageOutroFinished()
    {
        bCanShowMessage = true;
    }
}
