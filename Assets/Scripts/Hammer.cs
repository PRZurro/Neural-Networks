using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour {

    private Animator m_anim;
    private bool m_isMoved;
    private Vector3 m_target;
    public float m_movementSpeed {get; set;}
    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isMoved) {
            GoTo();
        }
    }

    private void GoTo() {
        m_isMoved = true;
        this.transform.position = Vector3.MoveTowards(this.transform.position, m_target, m_movementSpeed);
        if (Vector3.Distance(this.transform.position, m_target) < 0.1f) {
            m_isMoved = false;
        }
    }

    public void MoveTo(Vector3 target) {
        m_target = target;
    }

    public void Hit() {
        m_anim.SetBool("isHit", true);
        Invoke("Idle", 0.2f);
    }

    private void Idle() {
        m_anim.SetBool("isHit", false);
    }

}
