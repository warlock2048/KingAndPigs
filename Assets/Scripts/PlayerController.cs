using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Components
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;

    //Values
    [SerializeField] private float speed;
    private int direction = 1;

    //Param
    private int idSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_transform = GetComponent<Transform>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        ConfigParamId();
    }

    private void ConfigParamId()
    {
        idSpeed = Animator.StringToHash("Speed");
    }

    private void Update()
    {
        SetAnimationValues();
    }

    private void SetAnimationValues()
    {
        //Mathf.Abs convierte cualquier numero a positivo
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody2D.linearVelocityX));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Flip();
        m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, m_rigidbody2D.linearVelocityY);
    }

    private void Flip()
    {
        if (m_gatherInput.ValueX * direction < 0) 
        {
            m_transform.localScale = new Vector3(-m_transform.localScale.x, 1, 1);
            direction *= -1;
        }
    }
}
