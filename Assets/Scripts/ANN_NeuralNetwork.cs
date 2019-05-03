using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_NeuralNetwork : MonoBehaviour
{
    // Start is called before the first frame update

    public ANN_Layer m_Input , m_Hide, m_Output;

    void Start()
    {
        m_Input = new ANN_Layer(3,"Input");
        m_Hide = new ANN_Layer(5, "Hide");
        m_Output = new ANN_Layer(1, "Output");

        m_Input.AddRelations(null, m_Hide);
        m_Hide.AddRelations(m_Input, m_Output);
        m_Output.AddRelations(m_Hide,null);

        m_Input.DebugInfo();
        m_Hide.DebugInfo();
        m_Output.DebugInfo();

        Debug.Log(m_Hide.m_parentLayer.m_layerName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
