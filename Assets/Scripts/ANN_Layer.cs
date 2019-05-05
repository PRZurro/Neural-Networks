using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_Layer
{
    string m_layerName;
    public int m_numberOfParentNeurons, m_numberOfNeurons, m_numberOfChildNeurons;
    public ANN_Layer m_childLayer, m_parentLayer;

    public float[,] m_weight;
    public float[] m_biasWeight;

    public float[] m_errors;
    public float[] m_desiredValues;
    public float[] m_neuronValues;

    public float[,] m_weightsIncrease;   
    public float[] m_biasValues;


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
            m_weightsIncrease = new float[m_numberOfNeurons, m_numberOfChildNeurons];
            m_biasWeight = new float[m_numberOfChildNeurons];
            m_biasValues = new float[m_numberOfChildNeurons];
        }

    }

    public void RamdomWeight()
    {
        if (m_childLayer != null)
        {
            for (int j = 0; j < m_numberOfChildNeurons; j++) {
                m_biasWeight[j] = Random.Range(-1f, 1f);
                for (int i = 0; i < m_numberOfNeurons; i++) {
                    m_weight[i, j] = Random.Range(-1f, 1f);
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
                x += m_parentLayer.m_biasValues[j] * m_parentLayer.m_biasWeight[j];

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
            
            switch (Const.USE_MOMENTUM){
                case false:
                    FitErrorWeight(Const.LEARNING_RATIO);
                    break;
                case true:
                    FitErrorWeightWithMomentum(Const.LEARNING_RATIO, Const.MOMENTUM_RATIO);
                    break;
            }
            FitErrorBias(Const.RATIO_APRENDIZAJE);
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
    public float ECM_ErrorCuadraticMedium() {
        float error = 0;
        for (int i = 0; i < m_numberOfNeurons; i++) {
            error += Mathf.Pow(m_neuronValues[i] - m_desiredValues[i], 2);
        }
        error /= m_numberOfNeurons;
        return error;
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

    void FitErrorWeight(float learningRatio) {
        for (int i = 0; i < m_numberOfNeurons; i++) {
            for (int j = 0; j < m_numberOfChildNeurons; j++) {
                m_weight[i, j] += learningRatio * m_childLayer.m_errors[i] * m_neuronValues[i];
            }
        }       
    }

    void FitErrorWeightWithMomentum(float learningRatio, float momentumRatio) {
        for (int i = 0; i < m_numberOfNeurons; i++) {
            for (int j = 0; j < m_numberOfChildNeurons; j++) {
                float dw = learningRatio * m_childLayer.m_errors[j] * m_neuronValues[j];
                m_weight[i, j] += dw + momentumRatio * m_weightsIncrease[i, j];
                m_weightsIncrease[i, j] = dw;
            }
        }
    }

    void FitErrorBias(float learningRatio) {
        for (int i = 0; i < m_numberOfChildNeurons; i++) {
            m_biasWeight[i] += learningRatio* m_childLayer.m_errors[i] * m_biasValues[i];
        }
    }

    #endregion
    //----------------------------------------------------------------------------------------------------
    #region DebugUtility

    public string String()
    {
        //string parent = m_parentLayer.m_layerName;
        string parent = "NULL";
        string child = "NULL";
        string weight = "NULL";
        string weightIncrease = "NULL";
        string desired = "NULL";
        string biasWeight = "NULL";
        string biasValues = "NULL";

        if (m_parentLayer != null)
        {
            parent = m_parentLayer.m_layerName;

        }

        if (m_childLayer != null)
        {
            child = m_childLayer.m_layerName;
            weight = ArrayBiToTable(m_weight);
            weightIncrease = ArrayBiToTable(m_weightsIncrease);
            desired = ArrayToTable(m_desiredValues);
            biasWeight = ArrayToTable(m_biasWeight);
            biasValues = ArrayToTable(m_biasValues);
        }

        string info =
            "Name: " + m_layerName + "\n" +                    

            "Parent: " + parent + " have " + m_numberOfParentNeurons + " neuron" +
            " ==> This have " + m_numberOfNeurons + " neuron" + 
            " ==> Child: " + child + " have " + m_numberOfChildNeurons + " neuron" + "\n" +
                        
            "Weight: " + "\n" + weight + "\n" +
            "Weight Increase: " + "\n" + weightIncrease + "\n" +
            "Desired: " + "\n" + desired + "\n" +
            "Bias Weight: " + "\n" + biasWeight + "\n" +
            "Bias Values: " + "\n" + biasValues + "\n" +
            "Errors: " + "\n" + ArrayToTable(m_errors) + "\n" +
            "Desire Values: " + "\n" + ArrayToTable(m_desiredValues) + "\n" +
            "Neuron Values: " + "\n" + ArrayToTable(m_neuronValues) + "\n"
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
