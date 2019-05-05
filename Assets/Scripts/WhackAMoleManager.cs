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

    [SerializeField]
    bool m_manualTraining;

    private string m_AIInputParameters = "";
    private string m_AIOutputParameters = "";

    List<Mole> m_moles;
    List<byte> m_hiddenMoles;
    List<byte> m_unhiddenMoles;

    int m_score;

    float m_curTimer;
    float m_curTime;

    void Awake()
    {
        m_hammer.MovementSpeed = m_gameSettings.hammerSpeed;

        // Set the cameras when this manager is controlled by a human player
        if(m_humanPlayable)
        {
            Camera.main.depth = -90;
            Camera.main.tag = "Untagged";
    
            m_camera.tag = "MainCamera";
        }

        m_moles = new List<Mole>();
        m_hiddenMoles = new List<byte>();
        m_unhiddenMoles = new List<byte>();
        m_score = 0;

        // Save the moles that are in the first child of this transform
        Transform molesParent = transform.GetChild(0);

        byte i = 0;

        foreach (Transform moleTr in molesParent)
        {
            m_moles.Add(moleTr.GetComponent<Mole>());
            m_moles[i].Initialize(i, GetHidingMole, m_gameSettings.moleSettings[0], m_gameSettings.moleSettings,m_gameSettings.moleSettingsProbabilities);
            m_hiddenMoles.Add(i);

            i++;
        }

        ResetTimers();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) {
            Debug.Log(m_AIInputParameters);
            Debug.Log(m_AIOutputParameters);
            
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            GetSceneState();
        }
        m_curTime += Time.deltaTime;

        ManageMoleAvailability();

        ManagePlayerInput();
    }

    /// <summary>
    /// Send the hammer to hit a mole
    /// </summary>
    public void HitMole(Mole mole)
    {
        m_hammer.HitOn(mole.position());
    }

    /// <summary>
    /// Send the hammer to hit a mole by id
    /// </summary>
    /// <param name="moleID"></param>
    public void HitMole(byte moleID)
    {
        Mole mole = GetMole(moleID);
        HitMole(mole);
    }

    /// <summary>
    /// Manage the moles availability by setting the number of moles that has to be available randomly
    /// </summary>
    void ManageMoleAvailability()
    {
        // When timer is reached reset the timers and set the number of available moles randomly
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

    /// <summary>
    /// Function similar to a controller, to manage the human player input, that includes the creation of raycasts, 
    /// the handling of the raycasts and send the hammer to hit on a hole by left mouse clicks
    /// </summary>
    void ManagePlayerInput()
    {
        if (m_humanPlayable && Camera.main == m_camera)
        {
            // When left mouse button is clicked...
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // The ray only hits the layers hole and collisionable
                int layerMask = LayerMask.GetMask("Hole", "Collisionable");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    Mole mole =  hit.transform.parent.GetComponent<Mole>();

                    if(mole)
                    {
                        m_hammer.HitOn(mole.position());
                        manualTraining(mole.ID);
                    }
                    else
                    {
                        mole = hit.transform.GetComponent<Mole>();

                        if (mole)
                        {
                            m_hammer.HitOn(mole.position());
                            manualTraining(mole.ID);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Set a number of moles available
    /// </summary>
    /// <param name="nMoles"></param>
    void SetAvailableMoles(int nMoles)
    {
        for (int i = 0; i < m_hiddenMoles.Count && i < nMoles; i++)
        {
            UnhideMole(m_hiddenMoles[Random.Range(0, m_hiddenMoles.Count)]);
        }
    }

    /// <summary>
    /// Method called by a delegate when an instance of Mole class is hiding  
    /// </summary>
    /// <param name="moleID"></param>
    /// <param name="collision"></param>
    void GetHidingMole(byte moleID, bool collision)
    {
        if(collision)
        {         
            m_score += m_moles[moleID].score();
            m_scoreText.text = m_score.ToString();
        }

        StartCoroutine(MakeMoleAvailable(moleID, 0.5f));
    }
    
    /// <summary>
    /// Unhide a mole by id
    /// </summary>
    /// <param name="moleID"></param>
    void UnhideMole(byte moleID)
    {
        m_hiddenMoles.Remove(moleID);
        m_unhiddenMoles.Add(moleID);

        m_moles[moleID].Unhide(m_gameSettings.moleSettings, m_gameSettings.moleSettingsProbabilities);
    }

    /// <summary>
    /// Reset the timers
    /// </summary>
    void ResetTimers()
    {
        m_curTime = 0.0f;
        m_curTimer = Random.Range(m_gameSettings.minAppearanceTime, m_gameSettings.maxAppearanceTime) * 10.0f;
    }

    /// <summary>
    /// Make a mole by id available in the input time  
    /// </summary>
    /// <param name="moleID"></param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    IEnumerator MakeMoleAvailable(byte moleID, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        m_unhiddenMoles.Remove(moleID);
        m_hiddenMoles.Add(moleID);
    }

    ///////////////////////////--GETTERS--////////////////////////////

    /// <summary>
    /// Get mole by the id 
    /// </summary>
    /// <param name="moleID"></param>
    /// <returns></returns>
    public Mole GetMole(byte moleID)
    {
        return m_moles[moleID];
    }

    void manualTraining(byte ID) {
        if (m_manualTraining) {
            ANN_MoleInput m_AnnMoleInput;
            m_AIInputParameters += "{";
            m_AIInputParameters += "{";
            for (int i = 0; i < m_moles.Count; i++) {
                
                Mole mole = m_moles[i];
                m_AnnMoleInput = new ANN_MoleInput(mole.isShiny(), mole.GetMoleType(), Vector3.Distance(m_moles[i].position(), m_hammer.transform.position), mole.isHidden());
                

                m_AIInputParameters += m_AnnMoleInput.GetString() + ", ";
            }

            m_AIInputParameters += "}, \n";
        }
        m_AIOutputParameters += "{" + ID + "} , " ;

    }

    public float[] GetSceneState() {
        float[] scenestate = new float[(System.Enum.GetNames(typeof(MoleType)).Length + 3) * m_moles.Count];
        string str = "";
        ANN_MoleInput m_AnnMoleInput;

        for (int i = 0; i < m_moles.Count; i++) {
            Mole mole = m_moles[i];            
            m_AnnMoleInput = new ANN_MoleInput(mole.isShiny(), mole.GetMoleType(), Vector3.Distance(m_moles[i].position(), m_hammer.transform.position), mole.isHidden());
            scenestate = m_AnnMoleInput.GetArray();

            str += m_AnnMoleInput.GetString();
        }
        //Debug.Log(str);
        return scenestate;
    }

    public void HitIDMole(int ID) {
        if (!m_humanPlayable) {
            m_hammer.HitOn(m_moles[ID].position());
        }       
    }

}
