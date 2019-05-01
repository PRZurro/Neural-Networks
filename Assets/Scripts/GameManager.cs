using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    Hammer m_hammer;

    List<Mole> m_moles;
    List<int> m_availableMoles;
    List<int> m_occupiedMoles;

    int m_score;

    void Awake()
    {
        m_score = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        ManageMoleAvailability();



    }

    public void HitMole(Mole mole)
    {
        // m_hammer.Hit(mole.position());
    }

    public void HitMole(int moleID)
    {
        Mole mole = GetMole(moleID);
        // m_hammer.Hit(mole.position());
    }

    public void ManageMoleAvailability()
    {
    }

    //////////////////////////////-GETTERS-////////////////////////////

    public Mole GetMole(int moleID)
    {
        return m_moles[moleID];
    }

    public List<int> AvailableMoles()
    {
        return m_availableMoles;
    }

    public List<int> OccuppiedMoles()
    {
        return m_occupiedMoles;
    }
}
