using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialNeuralNetwork
{
    public ANN_Layer m_input, m_hide, m_output;

    public ArtificialNeuralNetwork(int inputNumberOfNeurons, int hideNumberOfNeurons, int outputNumberOfNeurons)
    {
        m_input = new ANN_Layer(inputNumberOfNeurons, "Input");
        m_hide = new ANN_Layer(hideNumberOfNeurons, "Hide");
        m_output = new ANN_Layer(outputNumberOfNeurons, "Output");

        m_input.AddRelations(null, m_hide);
        m_hide.AddRelations(m_input, m_output);
        m_output.AddRelations(m_hide, null);

        m_input.RamdomWeight();
        m_hide.RamdomWeight();

    }

    bool InputNetwork(float[] value)
    {
        if (value.Length == m_input.m_numberOfNeurons)
        {
            m_input.m_neuronValues = value;
            return true;
        }
        return false;
    }

    float[] GetOutput()
    {
        return m_output.m_neuronValues;
    }

    public void FitNetwork(float[,] inputTraining, float[,] desire, int maxEpoch, float errorTolerance)
    {
        if (inputTraining.GetLength(1) == m_input.m_numberOfNeurons &&
            desire.GetLength(1) == m_output.m_numberOfNeurons &&
            inputTraining.GetLength(0) == desire.GetLength(0))
        {
            float error = errorTolerance + 1;
            int actualEpoch = 0;
            while ((error > errorTolerance) && (actualEpoch < maxEpoch))
            {
                error = 0;
                ++actualEpoch;
                for (int i = 0; i < inputTraining.GetLength(0); i++)
                {
                    InputNetwork(Utilities.ExtractRowFromBidimensionalMatrix(inputTraining, i));
                    SetDesiredOutput(Utilities.ExtractRowFromBidimensionalMatrix(desire, i));
                    FeedForward();
                    error += CalculateError();
                    BackPropagation();
                }

                error /= inputTraining.GetLength(0);
            }
            Debug.Log("First Training: Final Error: " + error + " Final Epoch: " + actualEpoch);
        }
        else
        {
            Debug.Log("Invalid Training Size");
        }
    }

    bool SetDesiredOutput(float[] value)
    {
        if (value.Length == m_output.m_numberOfNeurons)
        {
            m_output.m_desiredValues = value;
            return true;
        }
        return false;
    }

    void FeedForward()
    {
        m_input.ObtainNeuronValues();
        m_hide.ObtainNeuronValues();
        m_output.ObtainNeuronValues();
    }

    void BackPropagation()
    {
        m_output.ObtainErrors();
        m_hide.ObtainErrors();

        m_hide.AdjustWeight();
        m_output.AdjustWeight();
    }

    int GetMaxOutputID()
    {
        int id = -1;
        float max = float.MinValue;

        for (int i = 0; i < m_output.m_numberOfNeurons; i++)
        {
            if (m_output.m_neuronValues[i] > max)
            {
                max = m_output.m_neuronValues[i];
                id = i;
            }
        }
        return id;
    }

    float CalculateError()
    {
        return m_output.ECM_ErrorCuadraticMedium();
    }

    public void ShowLayers()
    {
        Debug.Log(m_input.String());
        Debug.Log(m_hide.String());
        Debug.Log(m_output.String());
    }

    public int ObtainAction(float[] input)
    {
        InputNetwork(input);
        FeedForward();
        return GetMaxOutputID();
    }
}
