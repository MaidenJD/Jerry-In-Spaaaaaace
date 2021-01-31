using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        bool success = GameManager.GetGameManager(out GameManager manager);

        if(!success)
        {
            GameManager.GameManagerLoaded += OnGameManagerLoaded;
        }
    }

    private void OnGameManagerLoaded(GameManager loadedGM)
    {
        GameManager.GameManagerLoaded -= OnGameManagerLoaded;
    }
}
