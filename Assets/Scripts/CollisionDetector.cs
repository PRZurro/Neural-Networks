using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class CollisionDetector : MonoBehaviour {

    public delegate void CollisionCommunication( Collision other );

    //[SerializeField]
    public CollisionCommunication m_communication;


    /// <summary>

    /// On collision2d enter...

    /// </summary>

    /// <param name="collision"></param>

    void OnCollisionEnter(Collision collision) {
        //Debug.Log("Hola soy un collision enter");
        m_communication( collision );

    }


    /// <summary>

    /// Set the delegate's function

    /// </summary>

    /// <param name="communication"></param>

    public void SetCollisionCommunication(CollisionCommunication communication) {
        m_communication = communication;

    }

}