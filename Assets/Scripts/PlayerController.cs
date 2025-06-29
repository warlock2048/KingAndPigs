using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player components
    private Rigidbody2D m_rigidbody2D;
    private GatherInput m_gatherInput;
    private Transform m_transform;
    private Animator m_animator;

    //Values
    [Header("Move and Jump settings")]
    [SerializeField] private float speed;
    private int direction = 1;
    [SerializeField] private float jumpForce;
    [SerializeField] private int extraJumps;
    [SerializeField] private int counterExtraJumps;
    

    //Player external components
    [Header("Ground settings")]
    [SerializeField] private Transform rFoot;
    [SerializeField] private Transform lFoot;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayLength;
    [SerializeField] private LayerMask groundLayer;

    //Param
    private int idSpeed;
    private int idIsGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_gatherInput = GetComponent<GatherInput>();
        m_transform = GetComponent<Transform>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        ConfigParamId();

        lFoot = GameObject.Find("LFoot").GetComponent<Transform>();
        rFoot = GameObject.Find("RFoot").GetComponent<Transform>();
    }

    private void ConfigParamId()
    {
        idSpeed = Animator.StringToHash("Speed");
        idIsGrounded = Animator.StringToHash("isGrounded");
    }

    private void Update()
    {
        SetAnimationValues();
    }

    private void SetAnimationValues()
    {
        //Mathf.Abs convierte cualquier numero a positivo
        m_animator.SetFloat(idSpeed, Mathf.Abs(m_rigidbody2D.linearVelocityX));
        m_animator.SetBool(idIsGrounded, isGrounded);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Jump();
        CheckGround();
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

    private void Jump()
    {
        if (m_gatherInput.IsJumping) 
        {
            if (isGrounded)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
            }
            if(counterExtraJumps > 0)
            {
                m_rigidbody2D.linearVelocity = new Vector2(speed * m_gatherInput.ValueX, jumpForce);
                counterExtraJumps--;
            }
        }
        m_gatherInput.IsJumping = false;
    }

    private void CheckGround()
    {
        RaycastHit2D lfootRay = Physics2D.Raycast(lFoot.position, Vector2.down, rayLength, groundLayer);
        RaycastHit2D rfootRay = Physics2D.Raycast(rFoot.position, Vector2.down, rayLength, groundLayer);

        if(lfootRay || rfootRay)
        {
            isGrounded = true;
            counterExtraJumps = extraJumps;
        }
        else
        {
            isGrounded= false;
        }
    }

}
