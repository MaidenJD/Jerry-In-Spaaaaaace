using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLevel : MonoBehaviour
{
    public string LevelToLoadTo;

    public void LoadLevel()
    {
        SceneManager.LoadSceneAsync(LevelToLoadTo, LoadSceneMode.Additive);

        SceneManager.UnloadSceneAsync(gameObject.scene);
    }



}
