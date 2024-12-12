using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    // Basic Player movement
    public float horiz;
    public float scale;
    private float vertic;
    public float speed = 2f;
    public float jumpCounter = 1;
    public float jumpPower = 16f;
    private Vector2 direction;
    public bool isFacingRight = true;
    public bool knockBacked;
    //knockback
    public float KB;

    //Animator
    public Animator animator;
    public Animator vfxAnimatior;

    public bool canAttack = true;

    // Ground checking
    public LayerMask groundMask;
    public bool grounded;
    public bool isCarry;
    public BoxCollider2D groundCheck;

    private AudioManager audioManager;

    // Player body
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        //if (grounded)
        //{
        //    rb.gravityScale = 4;
        //}
        if (knockBacked == false)
        {
            if (Input.GetButtonDown("Jump") && jumpCounter < 2)
            {
                Jump();

                
            }

            if(canAttack)
            {
                // Attack Function
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    audioManager.PlaySFX(audioManager.attackClip);
                    canAttack = false;
                    animator.SetTrigger("Attack");
                    vfxAnimatior.SetTrigger("Attack");
                    StartCoroutine(AttackCoolDown());
                }
            }

        }

    }
    private void FixedUpdate()
    {
        CheckGround();
        horiz = Input.GetAxisRaw("Horizontal");
        if(knockBacked == false)
        {
            Walk(horiz);
        }

    }

    // Walk function
    private void Walk(float walkInput)
    {
        // Walk Function order by direction input
        animator.SetFloat("Speed", MathF.Abs(walkInput));

        if (Mathf.Abs(walkInput) > 0)
        {
            if(walkInput > 0)
            {
                isFacingRight = true;
            }
            else if (walkInput < 0)
            {
                isFacingRight = false;
            }

            //Debug.Log(isFacingRight);
            Flip(walkInput, isFacingRight);
            rb.velocity = new Vector2(walkInput * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }

    // Jump function
    private void Jump()
    {
        audioManager.PlaySFX(audioManager.jumpClip);
        jumpCounter += 1;
        grounded = false ;
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        //rb.AddForce(new Vector2(rb.velocity.x, jumpPower));
    }

    // Ground checking function
    private void CheckGround()
    {
        //bool wasGrounded = grounded;
        //grounded = Physics2D.OverlapAreaAll(groundCheck.bounds.min, groundCheck.bounds.max, groundMask).Length > 0;
        //if (!wasGrounded && grounded)
        //{
        //    jumpCounter = 1;
        //}
        Vector2 groundCheckPosition = new Vector2(groundCheck.bounds.center.x, groundCheck.bounds.min.y);
        grounded = Physics2D.OverlapBox(groundCheckPosition, groundCheck.bounds.size, 0, groundMask);
        if (grounded)
        {
            jumpCounter = 1;
            if (isCarry)
            {
                return;
            }
            rb.gravityScale = 4;
        }

    }

    private void Flip(float walkinput, bool facingRight)
    {
        if (facingRight && walkinput > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = 1f;
            scale = localScale.x;
            transform.localScale = localScale;
        }
        else if (facingRight == false && walkinput < 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -1f;
            scale = localScale.x;
            transform.localScale = localScale;
        }
    }

    public void knockBack()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
    }

    private IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(1f);
        canAttack = true;
    }

}

