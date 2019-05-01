using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{

    private Animator m_anim;

    enum Hats {Mexican, King, Pirate };

    // Start is called before the first frame update
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
}
