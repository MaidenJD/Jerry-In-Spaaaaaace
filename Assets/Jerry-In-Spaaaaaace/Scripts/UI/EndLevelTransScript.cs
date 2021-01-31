using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTransScript : MonoBehaviour
{ 
    public Animator William;
    private bool Shown = false;
    // Start is called before the first frame update
    void Awake() {
    }
    void Start()
    {
        
        ShowOnTheRoad();
    }

    // Update is called once per frame
    void Update()
    {

    }
    //shows the level end screen
    public void ComeOnEileen(){
        if (Shown == false){
        William.SetTrigger("Open");
        Shown = true;
        }
    }
    //hides the level end screen and fades to black
    public void ComeOffEileen(){
        if (Shown){
        William.SetTrigger("Close");
        Shown = false;
        }
    }
    //Fades to Black
    public void BackInBlack(){
        William.SetTrigger("FadeToBlack");
    }
    //Fades in from Black
    public void LetThereBeLight(){
        William.SetTrigger("FadeToScreen");
    }
    //Fades in from Black and shows the Start Screen
    public void ShowOnTheRoad(){
        William.SetTrigger("OpenStart");
    }
    //Hides the Starting screen
    public void EngageLevel(){
        William.SetTrigger("CloseStart");
    }
    //Show just the Start Screen
    public void WhatWasTheObjectiveAgain(){
        William.SetTrigger("ShowStart");
    }
    //Shows GameOver Screen
    public void GameOverManGameOver(){
        William.SetTrigger("OpenOver");
    }
    //Hides GameOver Screen
    public void WellMeetAgain(){
        William.SetTrigger("CloseOver");
    }
}