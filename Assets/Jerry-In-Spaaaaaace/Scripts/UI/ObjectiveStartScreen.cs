using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveStartScreen : MonoBehaviour
{

    public GameObject objectiveImageObject;
    public GameObject objectiveTextObject;

    private Objective objectiveRef;
    private ObjectiveType currentObjectiveType;

    // Start is called before the first frame update
    void Start()
    {
        objectiveTextObject = GameObject.Find("ObjectiveText");
        objectiveImageObject = GameObject.Find("ObjectiveImg");

        objectiveRef = FindObjectOfType<Objective>();
        objectiveImageObject.GetComponent<Image>().sprite = objectiveRef.GetComponent<SpriteRenderer>().sprite;
        objectiveTextObject.GetComponent<Text>().text = objectiveRef.objectiveName;
    }
}
