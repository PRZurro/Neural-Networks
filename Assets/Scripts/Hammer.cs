using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    [SerializeField]
    bool isIA;

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
        // If it has a target move to it
        if (m_isMoving)
        {
            MoveTo();
        }
    }

    /// <summary>
    /// Do an animation of a hit
    /// </summary>
    void Hit()
    {
        m_anim.SetBool("isHitting", true);
        Invoke("GoIdle", 0.2f);
    }

    /// <summary>
    /// Set the flags to move to the target and hit
    /// </summary>
    /// <param name="target"></param>
    public void HitOn(Vector3 target)
    {
        m_target = target;
        m_isMoving = true;
    }

    /// <summary>
    /// Set the animation state to idle
    /// </summary>
    void GoIdle()
    {
        m_anim.SetBool("isHitting", false);
    }

    /// <summary>
    /// Move this object to the target and trigger the hit animation when the target is reached
    /// </summary>
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
