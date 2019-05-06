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

// {isShiny + isHidden + distance + nTypes }
[System.Serializable]
public class ANN_MoleInput
{
    public static int INPUTS_SIZE = System.Enum.GetNames(typeof(MoleType)).Length + 3;

    float[] m_arrayInputs = new float[INPUTS_SIZE];

    public ANN_MoleInput(bool isShiny, bool isHidden, float distance, MoleType moleType)
    {
        Set(isShiny, isHidden, distance, moleType);
    }

    public float[] GetArray()
    {
        return m_arrayInputs;
    }

    public override string ToString()
    {
        string str = "";
        str += m_arrayInputs[0];

        for (int i = 1; i < m_arrayInputs.Length; i++)
        {
            str += ", " + m_arrayInputs[i].ToString().Replace(",", ".") + "f";
        }

        return str;
    }

    public void Set(bool isShiny, bool isHidden, float distance, MoleType moleType)
    {
        m_arrayInputs[0] = Utilities.BoolToFloat(isShiny);
        m_arrayInputs[1] = Utilities.BoolToFloat(isHidden);
        m_arrayInputs[2] = distance / Const.MAX_DISTANCIA;

        for (int i = 3; i < m_arrayInputs.Length; i++)
        {
            m_arrayInputs[i] = 0.0f;
        }

        m_arrayInputs[(byte)moleType + 3] = 1.0f;
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

public class Const
{
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

public class Utilities
{
    public static float BoolToFloat(bool i)
    {
        if (i)
        {
            return 1.0f;
        }
        return 0.0f;
    }

    public static float[] ListToArray(List<float[]> list)
    {
        float[][] toOneDimension = list.ToArray();

        Debug.Log(list.ToArray().GetLength(1));

        int index = 0;
        int width = toOneDimension.GetLength(0);
        int height = toOneDimension.GetLength(1);
        
        float[] toReturn = new float[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, index++)
            {
                toReturn[index] = toOneDimension[x][y];
            }
        }

        return toReturn;
    }

    public static float[] ExtractRowFromBidimensionalMatrix(float[,] matrix, int row)
    {
        float[] unidimensional = new float[0];

        if (matrix.GetLength(0) > row)
        {
            unidimensional = new float[matrix.GetLength(1)];
            for (int i = 0; i < unidimensional.Length; i++)
            {
                unidimensional[i] = matrix[row, i];
            }
        }

        return unidimensional;
    }
}

