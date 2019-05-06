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
    public List<int> moleSettingsProbabilities; // Would be confusing that the sum of all probabilities weren't 100

    [Range(1.0f, 10.0f)]
    static public float shinyMultiplier = 2.0f;

    [Range(0.0f, 10.0f)]
    public float hammerSpeed = 5.0f;

    [Range(0.0f, 1.0f)] 
    public float minAppearanceTime = 0.2f, maxAppearanceTime = 10.0f; // 0 is 0 seconds, 1 is 10 seconds
}
