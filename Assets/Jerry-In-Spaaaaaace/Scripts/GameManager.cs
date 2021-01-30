using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string SelectScene;

    public IEnumerator WinLevel(string Scene, int Score = 0)
    {
        yield return new WaitForSeconds(5);

        SceneManager.UnloadSceneAsync(Scene);
        
        SceneManager.LoadSceneAsync(SelectScene, LoadSceneMode.Additive);
    }
}
