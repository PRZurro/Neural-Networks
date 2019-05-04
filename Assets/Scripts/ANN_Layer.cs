using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_Layer
{
    string m_layerName;
    int m_numberOfParentNeurons, m_numberOfNeurons, m_numberOfChildNeurons;
    public ANN_Layer m_childLayer, m_parentLayer;

    public float[,] m_weight;
    public float[] m_bias;

    public float[] m_errors;
    public float[] m_desiredValues;
    public float[] m_neuronValues;

    public float[,] m_weightsIncrease;   
    public float[] m_biasValues;

    //public float[,] pesosIncremento;
    //public float[] valoresNeuronas;
    //public float[] valoresDeseados;
    //public float[] errores;
    //public float[] biasValores;

    public ANN_Layer(int numberOfNeurons, string layerName)
    {
        m_layerName = layerName;

        m_numberOfNeurons = numberOfNeurons;

        m_errors = new float[m_numberOfNeurons];
        m_desiredValues = new float[m_numberOfNeurons];
        m_neuronValues = new float[m_numberOfNeurons];
    }

    public void AddRelations(ANN_Layer parent, ANN_Layer child)
    {
        if (parent != null)
        {
            m_parentLayer = parent;
            m_numberOfParentNeurons = parent.m_numberOfNeurons;
        }

        if (child != null)
        {
            m_childLayer = child;
            m_numberOfChildNeurons = child.m_numberOfNeurons;
            m_weight = new float[m_numberOfNeurons, m_numberOfChildNeurons];
            m_bias = new float[m_numberOfNeurons];
        }

    }

    public void RamdomWeight()
    {
        if (m_childLayer != null)
        {
            for (int j = 0; j < m_numberOfChildNeurons; j++)
            {

                m_bias[j] = Random.Range(-1f, 1f);

                for (int i = 0; i < m_numberOfNeurons; i++)
                {
                    m_weight[j, i] = Random.Range(-1f, 1f);
                }

            }
        }
    }

    public void ObtainNeuronValues()
    {
        if (m_parentLayer != null)
        {
            for (int j = 0; j < m_numberOfNeurons; j++)
            {
                float x = 0;
                for (int i = 0; i < m_numberOfParentNeurons; i++)
                {
                    x += m_parentLayer.m_neuronValues[i] * m_parentLayer.m_weight[i, j];
                }
                x += m_parentLayer.m_biasValues[j] * m_parentLayer.m_bias[j];

                if (m_childLayer == null && Const.OUTPUT_LINEAL)
                {
                    m_neuronValues[j] = x;
                }
                else
                {
                    m_neuronValues[j] = Sigmoide(x);
                }
            }
        }
    }

    public void ObtainErrors() {
        // if is Output Layer
        if (m_childLayer == null) {            
                CalculateOutputError();         
        }
        //if is Hide Layer
        else if (m_parentLayer != null) {
            CalculateHideError();
        }
    }

    public void AdjustWeight() {
        //If Input or Hide
        if (m_childLayer != null) {
            for (int i = 0; i < m_numberOfNeurons; i++) {
                for (int j = 0; j < m_numberOfChildNeurons; j++) {
                    // Formula de ajuste de peso
                    float dw = Const.RATIO_APRENDIZAJE * m_childLayer.m_errors[j] * m_neuronValues[j]; 

                    if (Const.USO_INERCIA) {
                        m_weight[i, j] += dw + Const.RATIO_INERCIA * m_weightsIncrease[i, j];
                        m_weightsIncrease[i, j] = dw;
                    }
                    else {
                        m_weight[i, j] += dw;
                    }
                }
            }
            for (int j = 0; j < m_numberOfChildNeurons; j++) {
                float dw = Const.RATIO_APRENDIZAJE * m_childLayer.m_errors[j] * m_biasValues[j];
                m_bias[j] += dw;
            }
        }
    }


    //----------------------------------------------------------------------------------------------------
    #region MathFunctions

    float Sigmoide(float x) {
        return 1.0f / (1 + Mathf.Exp(-x));
    }

    float SigmoideDerived(float x) {
        return Mathf.Exp(-x) / Mathf.Pow(1 + Mathf.Exp(-x), 2);
    }

    // Apuntes --> Computar el Error(1) 
    float ECM_ErrorCuadraticMedium() {
        return 5.0f;
    }

    // Apuntes --> Computar el Error(3) --> Derivada del output
    void CalculateOutputError() {
        for (int i = 0; i < m_numberOfNeurons; i++) {
            m_errors[i] = (m_desiredValues[i] - m_neuronValues[i]) * m_neuronValues[i] * (1 - m_neuronValues[i]);
        }
    }

    // Apuntes --> Computar el Error(4) --> Derivada de la capa oculta.
    void CalculateHideError() {
        for (int i = 0; i < m_numberOfNeurons; i++) {
            float suma = 0;
            for (int j = 0; j < m_numberOfChildNeurons; j++) {
                suma += m_childLayer.m_errors[j] * m_weight[i, j];
            }
            m_errors[i] = suma * m_neuronValues[i] * (1 - m_neuronValues[i]);
        }
    }

    #endregion
    //----------------------------------------------------------------------------------------------------
    #region DebugUtility

    public string String()
    {
        //string parent = m_parentLayer.m_layerName;
        string parent;
        string child;
        string weight = "";
        string bias = "";
        if (m_parentLayer != null)
        {
            parent = m_parentLayer.m_layerName.ToString();

        }

        if (m_childLayer != null)
        {
            child = m_childLayer.m_layerName.ToString();
            weight = ArrayBiToTable(m_weight);
            bias = ArrayToTable(m_bias);

        }

        string info =
            "Name: " + m_layerName + "\n" +

            "ParentNum: " + m_numberOfParentNeurons +
            " NeuronNum " + m_numberOfNeurons +
            " ChildNum " + m_numberOfChildNeurons + "\n" +

            "Parent: " + m_parentLayer + " Child: " + m_childLayer + "\n" +
            "Weight: " + "\n" + weight + "\n" +
            "Bias: " + "\n" + bias
            ;

        return info;
    }

    string ArrayToTable(float[] array) {
        string table = "";
        for (int i = 0; i < array.Length; i++) {
            table += array[i] + " // ";
        }
        return table;
    }

    string ArrayBiToTable(float[,] array) {
        string table = "";

        for (int i = 0; i < array.GetLength(0); i++) {
            table += "row " + i + " ";
            for (int j = 0; j < array.GetLength(1); j++) {
                table += "col" + i + " " + array[i, j] + " // ";
            }
            table += "\n";
        }

        return table;
    }

    #endregion
    //----------------------------------------------------------------------------------------------------
}
