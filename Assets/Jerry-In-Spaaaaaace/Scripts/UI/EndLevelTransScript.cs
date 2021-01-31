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
    public AudioClip LevelSpecificClip;

    private AudioSource Audio;

    private SpaceStation Station;

    public float ShowStartScreenTime = 4f;
    private bool Shown = false;

    private SpaceControls spaceControls;

    void Start()
    {
        spaceControls = new SpaceControls();

        ShowStartScreen();
        //Invoke(nameof(HideStartScreen), ShowStartScreenTime);

        Station = GameObject.FindObjectOfType<SpaceStation>();
        Audio = GetComponent<AudioSource>();
    }

    //shows the level end screen
    public void EnableLevelEndScreen(){
        if (Shown == false){
            William.SetTrigger("Open");
            Shown = true;

            var Clip = WinClips.Length > 0 ? WinClips[Mathf.RoundToInt(Random.Range(0, WinClips.Length))] : null;
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

        if (LevelSpecificClip != null)
        {
            Audio.PlayOneShot(LevelSpecificClip);
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
        William.SetTrigger("OpenOver");

        var Clip = LossClips.Length > 0 ? LossClips[Mathf.RoundToInt(Random.Range(0, LossClips.Length))] : null;
        if (Clip != null)
        {
            Audio.PlayOneShot(Clip);
        }
    }

    //Hides GameOver Screen
    public void HideGameOverScreen(){
        William.SetTrigger("CloseOver");
    }

    #region Callbacks
    private void StartDay()
    {
        StartDayButton.onClick.RemoveListener(StartDay);

        HideStartScreen();
    }
    #endregion
}
