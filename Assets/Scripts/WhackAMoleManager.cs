using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhackAMoleManager : MonoBehaviour
{
    [SerializeField]
    Hammer m_hammer;

    [SerializeField]
    Camera m_camera;

    [SerializeField]
    GameSettings m_gameSettings;

    [SerializeField]
    bool m_humanPlayable = false;

    [SerializeField]
    Text m_scoreText;

    List<Mole> m_moles;
    List<byte> m_availableMoles;
    List<byte> m_occupiedMoles;

    int m_score;

    float m_curTimer;
    float m_curTime;

    void Awake()
    {
        m_hammer.MovementSpeed = m_gameSettings.hammerSpeed;

        if(m_humanPlayable)
        {
            Camera.main.depth = -90;
            Camera.main.tag = "Untagged";
    
            m_camera.tag = "MainCamera";
        }
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
            m_availableMoles.Add(i);

            i++;
        }

        ResetTimers();
    }

    void Update()
    {
        m_curTime += Time.deltaTime;

        ManageMoleAvailability();

        ManagePlayerInput();
    }

    public void HitMole(Mole mole)
    {
        m_hammer.HitOn(mole.position());
    }

    public void HitMole(byte moleID)
    {
        Mole mole = GetMole(moleID);
        HitMole(mole);
    }

    void ManageMoleAvailability()
    {
        if (m_curTime >= m_curTimer)
        {
            ResetTimers();

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

    void ManagePlayerInput()
    {
        if (m_humanPlayable && Camera.main == m_camera)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                int layerMask = LayerMask.GetMask("Hole", "Collisionable");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    Mole mole =  hit.transform.parent.GetComponent<Mole>();

                    if(mole)
                    {
                        m_hammer.HitOn(mole.position());
                    }
                    else
                    {
                        mole = hit.transform.GetComponent<Mole>();

                        if (mole)
                        {
                            m_hammer.HitOn(mole.position());
                        }
                    }
                }
            }
        }
    }

    void SetAvailableMoles(int nMoles)
    {
        for (int i = 0; i < m_availableMoles.Count && i < nMoles; i++)
        {
            UnhideMole(m_availableMoles[Random.Range(0, m_availableMoles.Count)]);
        }
    }

    void GetHidingMole(byte moleID, bool collision)
    {
        if(collision)
        {
            Debug.Log("eeeey");
            m_score += m_moles[moleID].score();
            m_scoreText.text = m_score.ToString();
        }

        StartCoroutine(MakeMoleAvailable(moleID, 0.5f));
    }

    void UnhideMole(byte moleID)
    {
        m_availableMoles.Remove(moleID);
        m_occupiedMoles.Add(moleID);

        m_moles[moleID].Unhide(m_gameSettings.moleSettings, m_gameSettings.moleSettingsProbabilities);
    }

    void ResetTimers()
    {
        m_curTime = 0.0f;
        m_curTimer = Random.Range(m_gameSettings.minAppearanceTime, m_gameSettings.maxAppearanceTime) * 10.0f;
    }

    IEnumerator MakeMoleAvailable(byte moleID, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        m_occupiedMoles.Remove(moleID);
        m_availableMoles.Add(moleID);
    }

    ///////////////////////////--GETTERS--////////////////////////////

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
}
