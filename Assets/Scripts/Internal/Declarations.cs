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


public class ANN_MoleInput {

    //public float isShiny;
    //public float[] moleTypes = new float[System.Enum.GetNames(typeof(MoleType)).Length];
    //public float ditance;
    //public float isHidden;

    float[] m_arrayInputs = new float[System.Enum.GetNames(typeof(MoleType)).Length + 3];

    public ANN_MoleInput(bool isShiny, MoleType moleType, float distance, bool isHidden) {

        m_arrayInputs[0] = Utility.BoolToFloat(isShiny);
        
        m_arrayInputs[1] = Utility.BoolToFloat(isHidden);
        m_arrayInputs[2] = distance / Const.MAX_DISTANCIA;

        for (int i = 3; i < m_arrayInputs.Length; i++) {
            m_arrayInputs[i] = 0.0f;            
        }
        m_arrayInputs[(byte)moleType + 3] = 1.0f;

    }

    public float[] GetArray() {
        return m_arrayInputs;
    }

   public  string GetString() {
        string str = "";
        str += m_arrayInputs[0];
        for (int i = 1; i < m_arrayInputs.Length; i++) {
            str += ", " + m_arrayInputs[i].ToString().Replace(",", ".") + "f";
        }

       
        return str;
    }

};

[System.Serializable]
public struct MoleSettings
{
    public MoleType moleType;
    public int score;

    [Range(0.0f, 1.0f)]
    public float shinyProbability;

    [Range(0.5f, 25.0f)]
    public float timeToHide;
}


public class Const {
    public const bool OUTPUT_LINEAL = false;
    public const float RATIO_APRENDIZAJE = 0.3f; // Recomendado entre 0.25 y 0.5
    public const bool USO_INERCIA = true;
    public const float RATIO_INERCIA = 0.5f; // Recomendado entre 0 y 1.

    public const float MAX_DISTANCIA = 100;


    //Mias
    public const bool USE_MOMENTUM = true;
    public const float LEARNING_RATIO = 0.3f;
    public const float MOMENTUM_RATIO = 0.5f;

}

public class Utility{

    public static float BoolToFloat(bool i) {
    if (i) {
        return 1.0f;
    }
    return 0.0f;
    }
    
    
}

