using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLevel : MonoBehaviour
{
    public IEnumerator StartGame()
    {
        yield return SceneManager.LoadSceneAsync("LevelOne", LoadSceneMode.Additive);

        yield return SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}
