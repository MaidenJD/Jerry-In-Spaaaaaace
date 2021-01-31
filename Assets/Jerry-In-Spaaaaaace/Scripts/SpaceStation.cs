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
                gameManager.WinLevel(gameObject.scene.name);
            }
        }
    }
}