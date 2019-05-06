using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public CollisionCommunication m_communication;

    void OnCollisionEnter(Collision collision)
    {
        m_communication();
    }

    /// <summary>
    /// Set the delegate's function
    /// </summary>
    /// <param name="communication"></param>
    public void SetCollisionCommunication(CollisionCommunication communication)
    {
        m_communication = communication;
    }
}
