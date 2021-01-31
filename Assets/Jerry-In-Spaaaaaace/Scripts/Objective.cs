using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : Debris
{
    public ObjectiveType Type;
    public string objectiveName;

    private void Start()
    {
        switch (Type)
        {
            case ObjectiveType.ElonMuskCar:
                objectiveName = "The Tesla";
                break;
            case ObjectiveType.Lunchbox:
                objectiveName = "Jerry's Lunchbox";
                break;
            case ObjectiveType.Sputnik:
                objectiveName = "Sputnik";
                break;
        }
    }
}

public enum ObjectiveType
{
    ElonMuskCar,
    Sputnik,
    Lunchbox
}