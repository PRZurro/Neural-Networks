using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    Animator m_anim;
    bool m_isMoving;
    Vector3 m_target;

    public float MovementSpeed { get; set; }

    void Start()
    {
        m_anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (m_isMoving)
        {
            MoveTo();
        }
    }

    void Hit()
    {
        m_anim.SetBool("isHitting", true);
        Invoke("GoIdle", 0.2f);
    }

    public void HitOn(Vector3 target)
    {
        m_target = target;
        m_isMoving = true;
    }

    void GoIdle()
    {
        m_anim.SetBool("isHitting", false);
    }

    void MoveTo()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_target, MovementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, m_target) < 0.01f)
        {
            m_isMoving = false;
            Hit();
        }
    }
}
