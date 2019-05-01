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
    List<byte> m_availableMoles;
    List<byte> m_occupiedMoles;

    int m_score;

    float m_timer;

    void Awake()
    {
        m_moles = new List<Mole>();
        m_availableMoles = new List<byte>();
        m_occupiedMoles = new List<byte>();

        m_score = 0;

        Transform molesParent = transform.GetChild(0);

        byte i = 0;

        foreach (Transform moleTr in molesParent)
        {
            m_moles.Add(moleTr.GetComponent<Mole>());
            m_moles[i].Initialize(i, GetHidingMole, m_gameSettings.moleSettings[0]);

            i++;
        }
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        ManageMoleAvailability();
    }

    public void HitMole(Mole mole)
    {
        m_hammer.HitOn(mole.position());
    }

    public void HitMole(byte moleID)
    {
        Mole mole = GetMole(moleID);
        m_hammer.HitOn(mole.position());
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

    public Mole GetMole(byte moleID)
    {
        return m_moles[moleID];
    }

    public List<byte> AvailableMoles()
    {
        return m_availableMoles;
    }

    public List<byte> OccuppiedMoles()
    {
        return m_occupiedMoles;
    }

    void SetAvailableMoles(int nMoles)
    {
        for (int i = 0; i < m_availableMoles.Count && i < nMoles; i++)
        {
            UnhideMole((byte)Random.Range(0, m_availableMoles.Count));
        }
    }

    void GetHidingMole(byte moleID)
    {
        m_occupiedMoles.Remove(moleID);
        m_availableMoles.Add(moleID);
    }

    void UnhideMole(byte moleID)
    {
        m_availableMoles.Remove(moleID);
        m_occupiedMoles.Add(moleID);

        m_moles[moleID].Unhide(m_gameSettings.moleSettings, m_gameSettings.moleSettingsProbabilities);
    }
}
