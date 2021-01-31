using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameManager : MonoBehaviour
{
    private GameManager gm;
    public string MissionName;
    public float StartDelay = 3f;
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
        gm = loadedGM;
        GameManager.GameManagerLoaded -= OnGameManagerLoaded;
    }

    public void StartGame()
    {

        gm.StartMission(gameObject.scene.name, MissionName, StartDelay);
    }
}
