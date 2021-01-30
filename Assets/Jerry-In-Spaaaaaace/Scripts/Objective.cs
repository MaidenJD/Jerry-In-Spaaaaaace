using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : Debris
{
    public ObjectiveType Type;
}


public enum ObjectiveType
{
    ElonMuskCar,
    Sputnik,
    SpaceCore
}