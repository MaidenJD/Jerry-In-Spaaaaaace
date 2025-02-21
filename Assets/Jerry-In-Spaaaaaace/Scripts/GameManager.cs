using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string SelectScene;

    private Dictionary<string, int> sceneScores = new Dictionary<string, int>();

    private const string GameManagerScene = "GameManager";
    private const string MainMenuScene = "MainMenuScene";

    /// <summary>
    /// Attempts to Get the Game Manager, if the Scene isn't loaded it will return false and the GM Variable will be null
    /// </summary>
    /// <param name="GM">If Successful this will be the reference to GM</param>
    /// <returns>If true, it has retreved GameManager</returns>
    public static bool GetGameManager(out GameManager GM)
    {
        var scene = SceneManager.GetSceneByName(GameManagerScene);

        if(!scene.isLoaded)
        {
            SceneManager.sceneLoaded += WaitForGMScene;
            SceneManager.LoadScene(GameManagerScene, LoadSceneMode.Additive);
            scene = SceneManager.GetSceneByName(GameManagerScene);
        }

        GameObject[] objects = scene.GetRootGameObjects();

        for(int i = 0; i < objects.Length; i++)
        {
            GM = objects[i].GetComponent<GameManager>();
            if(GM != null)
            {
                return true;
            }
        }

        GM = null;
        Debug.Log($"Scene {GameManagerScene} was not loaded, Loading the GameManager now");
        return false;
    }

    /// <summary>
    /// Event for Listening to when the GameManager has been loaded
    /// </summary>
    public static System.Action<GameManager> GameManagerLoaded;

    private static void WaitForGMScene(Scene scene, LoadSceneMode sceneMode)
    {
        if(scene.name.Equals(GameManagerScene))
        {
            SceneManager.sceneLoaded -= WaitForGMScene;

            GameManager GM = null;
            GameObject[] objects = scene.GetRootGameObjects();

            for (int i = 0; i < objects.Length; i++)
            {
                GM = objects[i].GetComponent<GameManager>();
                if (GM != null)
                {
                    break;
                }
            }

            if (GM != null)
            {
                GameManagerLoaded.Invoke(GM);
            }
            else
            {
                Debug.LogError($"The {GameManagerScene} scene doesn't contain a GameManager Script, please add one");
            }
            //Clear the Delegate
            GameManagerLoaded = null;
        }
    }

    /// <summary>
    /// This Unloads the Won Scene and loads the selected scene and saves the score
    /// </summary>
    /// <param name="Scene">The Scene that has been won in</param>
    /// <param name="Score">The Score won in that scene</param>
    /// <param name="StartWaitTime">The amount of time the functions waits before unloading won scene</param>
    /// <returns></returns>
    public IEnumerator WinLevelAsync(string UnloadScene, string NextScene, int Score = 0, float StartWaitTime = 5f)
    {
        //Save the Score
        sceneScores.Add(UnloadScene, Score);

        //I think this is for the fade to black? 
        yield return new WaitForSeconds(StartWaitTime);

        //Unload the "Won" scene
        yield return SceneManager.UnloadSceneAsync(UnloadScene);
        
        if(string.IsNullOrEmpty(NextScene))
        {
            yield return SceneManager.LoadSceneAsync(MainMenuScene, LoadSceneMode.Additive);
        }
        else
        {
            //Load in the Next Scene
            yield return SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Additive);
        }
        
    }

    public void WinLevel(string Scene, string NextScene, int Score = 0, float StartWaitTime = 5f)
    {
        //Starting the coroutine here because it will unload the scene were the coroutine is called and will start after unloaded the scene it was called from
        StartCoroutine(WinLevelAsync(Scene, NextScene, Score, StartWaitTime));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(GoToMainMenuAsync());
    }

    private IEnumerator GoToMainMenuAsync()
    {
        List<Scene> removeScenes = new List<Scene>();

        int sceneCount = SceneManager.sceneCount;

        for(int i = 0; i < sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);

            if(scene.buildIndex != gameObject.scene.buildIndex)
            {
                removeScenes.Add(scene);
            }
        }

        for(int i = 0; i < removeScenes.Count; i++)
        {
            yield return SceneManager.UnloadSceneAsync(removeScenes[i]);
        }

        yield return SceneManager.LoadSceneAsync(MainMenuScene);
    }

    public void RestartMission(string sceneName)
    {
        StartCoroutine(RestartMissionAsync(sceneName));
    }

    private IEnumerator RestartMissionAsync(string name)
    {
        yield return SceneManager.UnloadSceneAsync(name);

        yield return SceneManager.LoadSceneAsync(name);
    }

    /// <summary>
    /// This is intended for loading from the MainMenu into a game scene 
    /// </summary>
    /// <param name="unloadScene"></param>
    /// <param name="MissionScene"></param>
    public void StartMission(string unloadScene, string MissionScene, float WaitTime = 0f)
    {
        StartCoroutine(StartMissionAsync(unloadScene, MissionScene, WaitTime));
    }

    private IEnumerator StartMissionAsync(string unloadScene, string MissionScene, float WaitTime = 0f)
    {
        if(WaitTime > Mathf.Epsilon)
        {
            yield return new WaitForSeconds(WaitTime);
        }

        yield return SceneManager.UnloadSceneAsync(unloadScene);

        yield return SceneManager.LoadSceneAsync(MissionScene);
    }
}
