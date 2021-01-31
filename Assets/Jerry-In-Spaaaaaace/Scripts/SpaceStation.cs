using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    public GameManager gameManager;
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

    private void Start()
    {
        StartPosition = transform.position;
        if(!GameManager.GetGameManager(out gameManager))
        {
            GameManager.GameManagerLoaded += OnGameManagerLoaded;
        }
        else
        {
            StartMission();
        }
    }

    private void OnGameManagerLoaded(GameManager newGameManager)
    {
        gameManager = newGameManager;
        StartMission();
    }

    private void StartMission()
    {
        if (!string.IsNullOrEmpty(StartingMessage))
        {
            var jerryMsg = FindObjectOfType<JerryMessage>();

            jerryMsg.ShowJerryMessage(StartingMessage);
        }
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
            Debug.Log($"collider hit {collision.gameObject.name}");

            var objectiveHit = collision.gameObject.GetComponent<Objective>() ?? null;

            if (objectiveHit != null)
            {
                won = true;
                gameManager.WinLevel(gameObject.scene.name, NextLevel);

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
        gameManager.WinLevel(gameObject.scene.name, NextLevel);

        if (!string.IsNullOrEmpty(WinMessage))
        {
            var jerryMsg = FindObjectOfType<JerryMessage>();

            jerryMsg.ShowJerryMessage(WinMessage);
        }
    }
}