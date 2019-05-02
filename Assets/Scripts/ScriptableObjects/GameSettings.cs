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

    [Range(0.0f, 10.0f)]
    public float hammerSpeed = 5.0f;

    [Range(0.0f, 1.0f)] 
    public float minAppearanceTime = 0.2f, maxAppearanceTime = 10.0f; // 0 is 0 seconds 1 is 10 seconds
}
