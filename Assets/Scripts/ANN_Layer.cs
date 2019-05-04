using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_Layer
{
    public string m_layerName;
    public int m_numberOfParentNeurons, m_numberOfNeurons, m_numberOfChildNeurons;
    public ANN_Layer m_childLayer, m_parentLayer;

    public float[,] m_weight;
    public float[] m_bias;

    public float[,] m_weightsIncrease;
    public float[] m_neuronValues;
    public float[] m_desiredValues;
    public float[] m_errors;
    public float[] m_biasValues;

    public ANN_Layer(int numberOfNeurons, string layerName)
    {
        m_numberOfNeurons = numberOfNeurons;
        m_layerName = layerName;
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

    public float Sigmoide(float x)
    {
        return 1.0f / (1 + Mathf.Exp(-x));
    }

    public void DebugInfo()
    {
        //string parent = m_parentLayer.m_layerName;
        string parent;
        string child;

        if (m_parentLayer != null)
        {
            parent = m_parentLayer.m_layerName.ToString();

        }

        if (m_childLayer != null)
        {
            child = m_childLayer.m_layerName.ToString();

        }

        Debug.Log(
            "Name: " + m_layerName + "\n" +

            "ParentNum: " + m_numberOfParentNeurons +
            " NeuronNum " + m_numberOfNeurons +
            " ChildNum " + m_numberOfChildNeurons + "\n" +

            "Parent: " + m_parentLayer + " Child: " + m_childLayer + "\n" +
            "Weight: " + m_weight + "\n" +
            "Bias: " + m_bias
            );

    }
}
