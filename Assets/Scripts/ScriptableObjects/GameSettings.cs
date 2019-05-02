using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
/// Scriptable objects to store some game settings
/// </summary>
[CreateAssetMenu(fileName = "New_Game_Settings", menuName = "Game Settings", order = 52)]
public class GameSettings : ScriptableObject
{
    public List<MoleSettings> moleSettings;
    public List<int> moleSettingsProbabilities; // Would be confusing that the summation of all probabilities weren't 100

    [Range(1, 10)]
    public int shinyMultiplier = 2;

    [Range(2.0f, 100.0f)]
    public float hammerSpeed = 10.0f;

    public float appearanceFrequency;
}
