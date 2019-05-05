using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ANN_Controller
{
    public int state;

    private ArtificialNeuralNetwork m_ANN;

    public ANN_Controller(int nInputLayerNeurons, int nHiddenLayerNeurons, int nOutputLayerNeurons)
    {
        m_ANN = new ArtificialNeuralNetwork(nInputLayerNeurons, nHiddenLayerNeurons, nOutputLayerNeurons);

        m_ANN.FitNetwork(TrainingDatabase.inputTraining1, TrainingDatabase.desiredOutputTraining1, 5000, 0.05f);
        ShowLayers();
    }

    public void UpdateANN(float[] sceneState)
    {
        state = m_ANN.ObtainAction(sceneState);
        Debug.Log(state);
    }

    void ShowLayers()
    {
        Debug.ClearDeveloperConsole();
        m_ANN.ShowLayers();
    }
}
