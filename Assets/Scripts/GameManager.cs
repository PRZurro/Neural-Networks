using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    Hammer m_hammer;

    [SerializeField]
    GameSettings m_gameSettings;

    List<Mole> m_moles;
    List<int> m_availableMoles;
    List<int> m_occupiedMoles;

    int m_score;

    float m_timer;

    void Awake()
    {
        m_score = 0;
    }

    void Start()
    {
        
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        ManageMoleAvailability();
    }

    public void HitMole(Mole mole)
    {
        //m_hammer.Hit(mole.position());
    }

    public void HitMole(int moleID)
    {
        Mole mole = GetMole(moleID);
        //m_hammer.Hit(mole.position());
    }

    void ManageMoleAvailability()
    {
        if(m_timer >= m_gameSettings.appearanceFrequency)
        {
            int nRandom = Random.Range(0, 100);

            if (nRandom < 60)
            {
                SetAvailableMoles(1);
            }
            else if (nRandom >= 60 && nRandom < 90)
            {
                SetAvailableMoles(2);
            }
            else
            {
                SetAvailableMoles(3);
            }
        }
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

    void SetAvailableMoles(int nMoles)
    {
        for (int i = 0; i < m_availableMoles.Count && i < nMoles; i++)
        {
            UnhideMole(Random.Range(0, m_availableMoles.Count));
        }
    }

    void UnhideMole(int moleID)
    {
        m_availableMoles.RemoveAt(moleID);
        m_occupiedMoles.Add(moleID);

        m_moles[moleID].Unhide(m_gameSettings.moleSettings, m_gameSettings.moleSettingsProbabilities);
    }
}
