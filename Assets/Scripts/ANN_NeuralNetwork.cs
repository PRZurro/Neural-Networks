using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_NeuralNetwork : MonoBehaviour {
    public ANN_Layer m_input, m_hide, m_output;

    struct ANN_MoleInput {
        MoleType type;
        bool isShyni;
        float ditance;
        bool isHiden;
    };

    void Start() {
        m_input = new ANN_Layer(3, "Input");
        m_hide = new ANN_Layer(5, "Hide");
        m_output = new ANN_Layer(1, "Output");

        m_input.AddRelations(null, m_hide);
        m_hide.AddRelations(m_input, m_output);
        m_output.AddRelations(m_hide, null);

        Debug.Log(m_input.String());
        Debug.Log(m_hide.String());
        Debug.Log(m_output.String());

        Debug.Log(m_hide.m_parentLayer.m_layerName);
    }
}
