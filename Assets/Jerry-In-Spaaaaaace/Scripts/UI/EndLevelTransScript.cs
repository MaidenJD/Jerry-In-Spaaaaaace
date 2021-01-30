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
    void Start()
    {
        LetThereBeLight();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ComeOnEileen(){
        if (Shown == false){
        William.SetTrigger("Open");
        Shown = true;
        }
    }
    public void ComeOffEileen(){
        if (Shown){
        William.SetTrigger("Close");
        Shown = false;
        }
    }
    public void BackInBlack(){
        William.SetTrigger("FadeToBlack");
    }
    public void LetThereBeLight(){
        William.SetTrigger("FadeToScreen");
    }
}