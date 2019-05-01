
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoleType : byte {
    NORMAL = 0,
    MEXICAN = 1,
    VIKING = 2,
    KING = 3
}



[System.Serializable]
public struct MoleSettings {
    public MoleType moleType;
    public int score;

    [Range(0.0f, 1.0f)]
    public float shinyProbability;

    [Range(0.5f, 5.0f)]
    public float timeToHide;
}

public class Mole : MonoBehaviour {

    public GameObject Hammer;

    MoleSettings settings;

    bool m_isAvailable;

    Vector3 m_position; // Hit target of the hammer

    private Animator m_anim;

    void Awake() {
        m_isAvailable = false;
        m_position = transform.GetChild(2).position; // Cambiar al pivote 
        transform.GetChild(1).GetComponent<CollisionDetector>().SetCollisionCommunication(ReciveCollision);
    }

    void ReciveCollision(Collision other) {
        Debug.Log(other.gameObject.name + " is Hit " + this.gameObject.name);

    }

    
    void Start() {
        m_anim = GetComponent<Animator>();

        //Hammer.GetComponent<Hammer>().Hit(m_position);
    }

    void Hide() {
        m_anim.SetBool("isHide", true);
    }

    void Unhide(List<MoleSettings> settingsList, List<int> itemProbability) {
        m_anim.SetBool("isHide", false);


    }

    void Update() {

    }

    public Vector3 position() {
        return m_position;
    }
}