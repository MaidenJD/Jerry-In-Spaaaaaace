using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class JerryMessage : MonoBehaviour
{
    private Animation Animations;
    private TMPro.TMP_Text Text;
    public float CloseDelay;
    public UnityEvent messageIntroDone;
    public UnityEvent messageGone;
    public UnityEvent messageStart;

    private AudioSource MessageAudio;
    public AudioClip[] IntroBeeps;
    public AudioClip[] OutroBeeps;

    private bool bCanShowMessage = true;

    void Start()
    {
        Animations = GetComponent<Animation>();
        Text = GetComponentInChildren<TMPro.TMP_Text>();

        MessageAudio = GetComponent<AudioSource>();

        //ShowJerryMessage("Sup Mother Fucker, I've had enough of your shit. I deserve Employee of the Month, DESERVE IT. I hear you joking about me back at the station. Just watch your back my friend, never know what can happen in space...");
    }

    public void ShowJerryMessage(string Message, float CloseDelay = 10f)
    {
        if (bCanShowMessage)
        {
            bCanShowMessage = false;

            messageStart.Invoke();

            Text.text = Message;
            Animations.Play("Incoming Message");
            this.CloseDelay = CloseDelay;
        }
    }

    private void PlayIntroMessageBeep()
    {
        var Clip = GetRandomBeep(IntroBeeps);
        if (Clip != null)
        {
            MessageAudio.PlayOneShot(Clip);
        }
    }

    private void PlayOutroMessageBeep()
    {
        var Clip = GetRandomBeep(OutroBeeps);
        if (Clip != null)
        {
            MessageAudio.PlayOneShot(Clip);
        }
    }

    private AudioClip GetRandomBeep(AudioClip[] Beeps)
    {
        return Beeps.Length > 0 ? Beeps[Mathf.RoundToInt(Random.Range(0, Beeps.Length))] : null;
    }

    public void MessageIntroFinished()
    {
        messageIntroDone.Invoke();
        StartCoroutine(DelayedMessageClosure(CloseDelay));
    }

    IEnumerator DelayedMessageClosure(float CloseDelay)
    {
        yield return new WaitForSeconds(CloseDelay);
        Animations.Play("Incoming Message Reverse");
        messageGone.Invoke();
    }

    public void MessageOutroFinished()
    {
        bCanShowMessage = true;
    }
}
