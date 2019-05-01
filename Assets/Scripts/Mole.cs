using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoleType : byte
{
    NORMAL = 0,
    MEXICAN = 1,
    VIKING = 2, 
    KING = 3
}

[System.Serializable]
public struct MoleSettings
{
    public MoleType moleType;
    public int score;

    [Range(0.0f, 1.0f)]
    public float shinyProbability;

    [Range(0.5f, 5.0f)]
    public float timeToHide;
}

public class Mole : MonoBehaviour
{
    bool m_isAvailable;

    Vector3 m_position;

    void Awake()
    {
        m_isAvailable = false;
        m_position = transform.position; // Cambiar al pivote 
    }

    private Animator m_anim;
    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    void Hide() {
        m_anim.SetBool("isHide", true);
    }

    void Unhide(Hats hat, bool isShainy) {
        m_anim.SetBool("isHide", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public Vector3 position()
    {
        return m_position;
    }
}
