using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTransScript : MonoBehaviour
{ 
    public Animator William;

    [Header("Start Screen")]
    public Button StartDayButton;
    [Header("End Screen")]
    public Button EndDayButton;
    [Header("Gameover Screen")]
    public Button RestartMissionButton;
    public Button ReturnToMainMenuButton;

    public AudioClip[] LossClips;
    public AudioClip[] WinClips;

    public bool IsFirstLevel = false;
    public AudioClip[] JerryClips;


    private AudioSource Audio;

    private SpaceStation Station;

    public float ShowStartScreenTime = 4f;
    private bool Shown = false;

    private SpaceControls spaceControls;

    void Start()
    {
        spaceControls = new SpaceControls();

        //Invoke(nameof(HideStartScreen), ShowStartScreenTime);

        Station = GameObject.FindObjectOfType<SpaceStation>();
        Audio = GetComponent<AudioSource>();

        ShowStartScreen();
    }

    //shows the level end screen
    public void EnableLevelEndScreen(){
        if (Shown == false){
            William.SetTrigger("Open");
            Shown = true;

            var Clip = GetRandomClip(WinClips);
            if (Clip != null)
            {
                Audio.PlayOneShot(Clip);
            }
        }
    }

    //hides the level end screen and fades to black
    public void DisableLevelEndScreen(){
        if (Shown){
            William.SetTrigger("Close");
            Shown = false;
        }
    }

    //Fades to Black
    public void FadeInBlack(){
        William.SetTrigger("FadeToBlack");
    }

    //Fades in from Black
    public void FadeOutBlack(){
        William.SetTrigger("FadeToScreen");
    }

    //Fades in from Black and shows the Start Screen
    public void ShowStartScreen(){
        William.SetTrigger("OpenStart");

        StartDayButton.onClick.AddListener(StartDay);

        if (IsFirstLevel)
        {
            var Clip = GetRandomClip(JerryClips);
            if (Clip != null)
            {
                Audio.PlayOneShot(Clip);
            }
        }
    }

    //Hides the Starting screen
    public void HideStartScreen(){
        William.SetTrigger("CloseStart");

        FindObjectOfType<SpaceStation>().StartMission();
    }

    //Show just the Start Screen
    public void WhatWasTheObjectiveAgain(){
        William.SetTrigger("ShowStart");
    }

    //Shows GameOver Screen
    public void ShowGameOverScreen(){
        if (Shown == false) {
            Shown = true;

            transform.Find("GameOverPanel").gameObject.SetActive(true);

            var Clip = GetRandomClip(LossClips);
            if (Clip != null)
            {
                Audio.PlayOneShot(Clip);
            }
        }
    }

    //Hides GameOver Screen
    public void HideGameOverScreen(){
        transform.Find("GameOverPanel").gameObject.SetActive(false);
    }

    private AudioClip GetRandomClip(AudioClip[] Clips)
    {
        return Clips.Length > 0 ? Clips[Mathf.RoundToInt(Random.Range(0, Clips.Length))] : null;
    }

    #region Callbacks
    private void StartDay()
    {
        StartDayButton.onClick.RemoveListener(StartDay);

        HideStartScreen();
    }
    #endregion
}
