using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField]
    Material m_normalMaterial, m_shinyMaterial;

    public Communication OnHide { get; set; }
    public byte ID { get; set; }

    MoleSettings m_settings;
    Vector3 m_position; // Hammer movement target

    Animator m_anim;

    List<int> m_accumulatedProbabilities;

    float m_curTime, m_curTimer;
    int m_score;

    bool m_isHidden;

    void Awake()
    {
        m_accumulatedProbabilities = new List<int>();

        m_position = transform.GetChild(2).position; // Change to pivot
        transform.GetChild(1).GetComponent<CollisionDetector>().SetCollisionCommunication(RecieveCollision);
        m_anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(!m_isHidden)
        {
            m_curTime += Time.deltaTime;

            // Hide when the timer has been reached
            if (m_curTime >= m_curTimer)
            {
                Hide();
            }
        }
    }

    /// <summary>
    /// Initialize this mole
    /// </summary>
    /// <param name="id"></param>
    /// <param name="onHide">Delegate to call when this mole is hidding</param>
    /// <param name="defaultSettings">default mole settings</param>
    public void Initialize(byte id, Communication onHide, MoleSettings defaultSettings)
    {
        ID = id;
        OnHide = onHide;
        m_settings = defaultSettings;
        ResetTimerToHide();
        m_isHidden = true;
        m_score = m_settings.score;
    }

    /// <summary>
    /// Hide the mole in the hole by animations
    /// </summary>
    /// <param name="collided">If this method was called after a collision with the hammer or not</param>
    public void Hide(bool collided = false)
    {
        if(!m_isHidden)
        {
            m_isHidden = true;
            m_anim.SetBool("isHidden", true);

            OnHide(ID, collided);
        }
    }

    /// <summary>
    /// Unhide the mole by animations, set the new type by the probabilities of each setting in the settings list
    /// </summary>
    /// <param name="settingsList"></param>
    /// <param name="settingsProbabilities">list of probabilities of each element in the settings list</param>
    public void Unhide(List<MoleSettings> settingsList, List<int> settingsProbabilities)
    {
        m_isHidden = false;
        
        // Fill the accumulated probabilities sequence if the input list does not have the same elements number
        if (settingsProbabilities.Count != m_accumulatedProbabilities.Count)
        {
            FillAccumulatedSequenceIn(settingsProbabilities, m_accumulatedProbabilities);
        }

        // Get a random MoleSettings objects in the settings list by each element probability

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

        // Set the animator state and reset timers
        m_anim.SetBool("isHidden", false);
        ResetTimerToHide();
    }

    /// <summary>
    /// Set the settings and enable the corresponding hat gameobject and 
    /// </summary>
    /// <param name="settings"></param>
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
            m_score = m_settings.score;

            if (Random.Range(0.0f, 1.0f) <= m_settings.shinyProbability)
            {
                transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = m_shinyMaterial;
                m_score = (int)(m_score * GameSettings.shinyMultiplier);
            }
            else
            {
                transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = m_normalMaterial;
            }
        }
    }

    /// <summary>
    /// Fill a list of integers with the accumulated sequence of another integer list
    /// </summary>
    /// <param name="numberSequence"></param>
    /// <param name="listToFill"></param>
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

    /// <summary>
    /// Recieve a collision from an external object thanks to delegates 
    /// </summary>
    void RecieveCollision()
    {
        Hide(true);
    }

    /// <summary>
    /// Reset the timer to hide when time's up
    /// </summary>
    void ResetTimerToHide()
    {
        m_curTime = 0.0f;
        m_curTimer = m_settings.timeToHide;
    }

    ///////////////////////////--GETTERS--////////////////////////////

    /// <summary>
    /// Return the position of the hammer's target
    /// </summary>
    /// <returns>This hammer movement target position</returns>
    public Vector3 position()
    {
        return m_position;
    }

    /// <summary>
    /// Returns the current score of this mole (type score and if is shiny multiplied by the shiny multiplier)
    /// </summary>
    /// <returns>This current score</returns>
    public int score()
    {
        return m_score;
    }
}
