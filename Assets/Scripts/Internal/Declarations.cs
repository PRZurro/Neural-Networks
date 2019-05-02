using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Communication(byte ID, bool collision);
public delegate void CollisionCommunication();

public enum MoleType : byte
{
    NORMAL = 0,
    EXPLORER = 1,
    SUMMER_DAY = 2,
    WIZARD = 3,
    COWBOY = 4,
    CROWN = 5,
    POKEMON_MASTER = 6,
    PROPELLER = 7
}

[System.Serializable]
public struct MoleSettings
{
    public MoleType moleType;
    public int score;

    [Range(0.0f, 1.0f)]
    public float shinyProbability;

    [Range(0.5f, 5.0f)]
    public float timeToHide;
}
