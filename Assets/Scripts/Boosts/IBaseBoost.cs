using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoostType
{
    ShockwaveBoost
}

interface IBaseBoost
{
    Vector3 startPosition { get; set; }
    bool hasBeenUsed { get; set; }
    string boostName { get; }

    void activateBoost();
    void resetBoost();
}
