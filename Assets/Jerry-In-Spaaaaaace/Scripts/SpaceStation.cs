using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    public GameManager GameManager;
    public float RotationSpeed;

    private Vector3 StartPosition;
    bool won = false;

    private void Start()
    {
        StartPosition = transform.position;
        GameManager = FindObjectOfType<GameManager>();
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
                StartCoroutine(GameManager.WinLevel(gameObject.scene.name));
            }
        }
    }
}