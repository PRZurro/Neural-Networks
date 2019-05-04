using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_NeuralNetwork : MonoBehaviour
{
    public ANN_Layer m_input , m_hide, m_output;

    void Start()
    {
        m_input = new ANN_Layer(3,"Input");
        m_hide = new ANN_Layer(5, "Hide");
        m_output = new ANN_Layer(1, "Output");

        m_input.AddRelations(null, m_hide);
        m_hide.AddRelations(m_input, m_output);
        m_output.AddRelations(m_hide,null);

        m_input.DebugInfo();
        m_hide.DebugInfo();
        m_output.DebugInfo();

        Debug.Log(m_hide.m_parentLayer.m_layerName);
    }
}
