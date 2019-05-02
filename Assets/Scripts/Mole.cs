
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoleType : byte
{
    NORMAL = 0,
    EXPLORER = 1,
    SUMMER_DAY = 2,
    WIZARD = 3,
    COWBOY = 4,
    CROWN = 5,
    POKEMON_MASTER = 6,
    PROPELLER = 7
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
    public delegate void Communication(byte ID);

    public Communication OnHide { get; set; }
    public byte ID { get; set; }

    [SerializeField]
    Material m_normalMaterial, m_shinyMaterial;

    MoleSettings m_settings;

    bool m_isAvailable;

    Vector3 m_position; // Hit target of the hammer

    Animator m_anim;

    List<int> m_accumulatedProbabilities;

    void Awake()
    {
        m_accumulatedProbabilities = new List<int>();

        m_isAvailable = false;
        m_position = transform.GetChild(2).position; // Change to pivot
        transform.GetChild(1).GetComponent<CollisionDetector>().SetCollisionCommunication(RecieveCollision);
        ID = 255;
        m_anim = GetComponent<Animator>();

        MoleSettings sett;
        sett.moleType = MoleType.PROPELLER;
        sett.score = 50;
        sett.shinyProbability = 0.9f;
        sett.timeToHide = 1;

        SetSettings(sett);


    }

    public void Initialize(byte id, Communication onHide, MoleSettings defaultSettings)
    {
        ID = id;
        OnHide = onHide;
        SetSettings(defaultSettings);
    }

    public void Hide()
    {
        m_anim.SetBool("isHide", true);
        m_isAvailable = true;
        OnHide(ID);
    }

    public void Unhide(List<MoleSettings> settingsList, List<int> itemProbability)
    {
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

        m_anim.SetBool("isHide", false);
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

    private void FillAccumulatedSequenceIn(List<int> numberSequence, List<int> toFill)
    {
        toFill.Clear();
        int accumulatedSum = 0;
        foreach (int probability in numberSequence)
        {
            accumulatedSum += probability;
            toFill.Add(accumulatedSum);
        }
    }

    void RecieveCollision()
    {
        Hide();
    }

    public Vector3 position()
    {
        return m_position;
    }
}