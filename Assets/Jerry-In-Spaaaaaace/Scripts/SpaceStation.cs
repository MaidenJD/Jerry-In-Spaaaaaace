using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    [Header("Component References")]
    public GameManager gameManager;

    public EndLevelTransScript endLevelTrans;

    public float RotationSpeed;

    [Header("Mission Details")]
    public Objective objective;
    [TextArea]
    public string StartingMessage;
    [TextArea]
    public string WinMessage;

    public string NextLevel;

    private Vector3 StartPosition;
    bool won = false;

    private PlayerInput playerRef;
    private JerryMessage jerryMsg;

    private void Start()
    {
        StartPosition = transform.position;
        if(!GameManager.GetGameManager(out gameManager))
        {
            GameManager.GameManagerLoaded += OnGameManagerLoaded;
        }
        else
        {
            //StartMission();
        }

        playerRef = FindObjectOfType<PlayerInput>();
        playerRef.InputEnabled = false;

        jerryMsg = FindObjectOfType<JerryMessage>();
    }

    private void OnEnable()
    {
        endLevelTrans.ReturnToMainMenuButton.onClick.AddListener(GoToMainMenu);
        endLevelTrans.RestartMissionButton.onClick.AddListener(RestartMission);
    }

    private void OnDisable()
    {
        endLevelTrans.ReturnToMainMenuButton.onClick.RemoveListener(GoToMainMenu);
        endLevelTrans.RestartMissionButton.onClick.RemoveListener(RestartMission);
    }

    private void OnGameManagerLoaded(GameManager newGameManager)
    {
        gameManager = newGameManager;
        //StartMission();
    }

    public void StartMission()
    {
        if (!string.IsNullOrEmpty(StartingMessage))
        {
            var jerryMsg = FindObjectOfType<JerryMessage>();

            jerryMsg.ShowJerryMessage(StartingMessage);
        }

        playerRef.InputEnabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, Time.deltaTime * RotationSpeed));

        if(transform.position != StartPosition)
            transform.position = StartPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!won)
        {
            //Debug.Log($"collider hit {collision.gameObject.name}");

            var objectiveHit = collision.gameObject.GetComponent<Objective>() ?? null;

            if (objectiveHit != null)
            {
                Win();
            }

            var debrisHIt = collision.gameObject.GetComponent<Debris>();

            if(debrisHIt != null)
            {
                //Get player ref from this debris
                PlayerInput shipRef = debrisHIt.GetPlayerShip();

                if(shipRef != null)
                {
                    //If this ship has more than one objective then we win
                    if(shipRef.ObjectiveCount > 0)
                    {
                        Win();
                    }
                }
            }

            var playerHit = collision.gameObject.GetComponent<PlayerInput>();

            if(playerHit != null)
            {
                if(playerHit.ObjectiveCount > 0)
                {
                    Win();
                }
            }
        }
    }

    private void Win()
    {
        won = true;
        playerRef.InputEnabled = false;

        if (!string.IsNullOrEmpty(WinMessage))
        {
            if(jerryMsg.ShowJerryMessage(WinMessage))
            {
                jerryMsg.messageGone.AddListener(FinishedWinTalking);
            }
            else
            {
                jerryMsg.messageFinished.AddListener(OnPreviousMessageFinished);
            }
        }
        else
        {
            //Show the end screen
            endLevelTrans.EnableLevelEndScreen();
            endLevelTrans.EndDayButton.onClick.AddListener(FinishDay);
        }
    }

    private void OnPreviousMessageFinished()
    {
        jerryMsg.messageFinished.RemoveListener(OnPreviousMessageFinished);

        if (!jerryMsg.ShowJerryMessage(WinMessage))
        {
            Debug.LogError("HELP ME");
            FinishMission();
        }
        else
        {
            jerryMsg.messageGone.AddListener(FinishedWinTalking);
        }
    }

    private void FinishedWinTalking()
    {
        var jerryMsg = FindObjectOfType<JerryMessage>();
        jerryMsg.messageGone.RemoveListener(FinishedWinTalking);

        //Show the end screen
        endLevelTrans.EnableLevelEndScreen();
        endLevelTrans.EndDayButton.onClick.AddListener(FinishDay);
    }

    private void FinishDay()
    {
        endLevelTrans.EndDayButton.onClick.RemoveListener(FinishDay);
        FinishMission();
    }

    /// <summary>
    /// This will start the transition to the next screen
    /// </summary>
    public void FinishMission()
    {
        gameManager.WinLevel(gameObject.scene.name, NextLevel, 0, 0f);
    }

    private void GoToMainMenu()
    {
        gameManager.GoToMainMenu();
    }

    private void RestartMission()
    {
        gameManager.RestartMission(gameObject.scene.name);
    }
}