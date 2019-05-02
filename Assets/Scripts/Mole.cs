using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    public Communication OnHide { get; set; }
    
    public byte ID { get; set; }

    MoleSettings m_settings;
    Vector3 m_position; // Hit target of the hammer

    Animator m_anim;

    List<int> m_accumulatedProbabilities;

    [SerializeField]
    Material m_normalMaterial, m_shinyMaterial;

    float m_curTime, m_curTimer;

    bool m_isHidden;

    void Awake()
    {
        m_accumulatedProbabilities = new List<int>();

        m_position = transform.GetChild(2).position; // Change to pivot
        transform.GetChild(1).GetComponent<CollisionDetector>().SetCollisionCommunication(RecieveCollision);
        m_anim = GetComponent<Animator>();
    }

    public void Initialize(byte id, Communication onHide, MoleSettings defaultSettings)
    {
        ID = id;
        OnHide = onHide;
        m_settings = defaultSettings;
        ResetTimerToHide();
        m_isHidden = true;
    }

    private void Update()
    {
        if(!m_isHidden)
        {
            m_curTime += Time.deltaTime;

            if (m_curTime >= m_curTimer)
            {
                Hide();
            }
        }
    }

    public void Hide(bool collided = false)
    {
        if(!m_isHidden)
        {
            m_isHidden = true;
            m_anim.SetBool("isHidden", true);

            OnHide(ID, collided);
        }
    }

    public void Unhide(List<MoleSettings> settingsList, List<int> itemProbability)
    {
        m_isHidden = false;

        if (itemProbability.Count != m_accumulatedProbabilities.Count)
        {
            FillAccumulatedSequenceIn(itemProbability, m_accumulatedProbabilities);
        }

        int maxProbability = m_accumulatedProbabilities[m_accumulatedProbabilities.Count - 1];
        int nRandom = Random.Range(0, maxProbability);
        int previousAccumulatedProbability = 0, currentIndex = 0;

        foreach (int accumulatedProbability in m_accumulatedProbabilities)
        {
            if (nRandom >= previousAccumulatedProbability && nRandom <= accumulatedProbability)
            {
                break;
            }
            else
            {
                previousAccumulatedProbability = accumulatedProbability;
            }

            currentIndex++;
        }

        SetSettings(settingsList[currentIndex]);

        m_anim.SetBool("isHidden", false);
        ResetTimerToHide();
    }


    public void SetSettings(MoleSettings settings)
    {
        if(m_settings.moleType != settings.moleType)
        {
            if(m_settings.moleType != MoleType.NORMAL)
            {
                transform.GetChild(1).GetChild(1).GetChild((int)m_settings.moleType - 1).gameObject.SetActive(false);
            }
            if(settings.moleType != MoleType.NORMAL)
            {
                transform.GetChild(1).GetChild(1).GetChild((int)settings.moleType - 1).gameObject.SetActive(true);
            }

            m_settings = settings;

            if (Random.Range(0.0f, 1.0f) <= m_settings.shinyProbability)
            {
                transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = m_shinyMaterial;
            }
            else
            {
                transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = m_normalMaterial;
            }
        }
    }

    private void FillAccumulatedSequenceIn(List<int> numberSequence, List<int> listToFill)
    {
        listToFill.Clear();
        int accumulatedSum = 0;
        foreach (int probability in numberSequence)
        {
            accumulatedSum += probability;
            listToFill.Add(accumulatedSum);
        }
    }

    void RecieveCollision()
    {
        Hide(true);
    }

    public Vector3 position()
    {
        return m_position;
    }

    public int score()
    {
        return m_settings.score;
    }

    void ResetTimerToHide()
    {
        m_curTime = 0.0f;
        m_curTimer = m_settings.timeToHide;
    }
}