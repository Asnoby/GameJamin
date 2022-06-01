using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyPathFinderScript : MonoBehaviour
{
    [SerializeField] private float detectrionRange = 5f;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpForce = 400f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector2 minMaxPatrol = new Vector2(-5, 5);
    [SerializeField] private LayerMask player;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Animator moveAnimator;

    private bool isGrounded;
    public bool alerted;
    private bool Home = true; 
    public float moveDir = 1;
    private float jumpTimer = 0.5f;
    private float waitTimer = 2f;
    private float alertTimer = 5f;
    private float startX;
    private float jumpEyes = 1;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startX = transform.position.x;
    }

    void Update()
    {
        //Checks how long the jumpeyes should be
        if (!alerted)
        {
            jumpEyes = 1;
        } else
        {
            jumpEyes = 1.5f;
        }


        //Checks if player is grounded
        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(0.2f, 0.5f), 0, ground);

        //Aggro stuff
        Debug.DrawRay(transform.position, new Vector2(moveDir, 0), Color.green);
       if (Physics2D.Raycast(transform.position, new Vector2 (moveDir, 0), detectrionRange, player))
        {
            alertTimer = 5;
            if (!alerted)
            {
                alerted = true;
                Home = false;
            }
        }

       if (alertTimer >= 0)
        {
            alertTimer -= 1 * Time.deltaTime;
        }else 
        {
            alerted = false;
            if (!Home)
            {
                if (transform.position.x <= startX)
                {
                    moveDir = 1;
                } else
                {
                    moveDir = -1;
                }

                if (transform.position.x >= startX -1 && transform.position.x <= startX + 1)
                {
                    Home = true;
                }
            }
        }

        //Jump if object 
        if (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.49f), new Vector2(moveDir, 0), jumpEyes, ground) && isGrounded 
            && jumpTimer <= 0 && !Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1.5f), new Vector2(moveDir, 0), jumpEyes, ground))
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpTimer = 1f;
        }

        //sets jumptimer
        if (jumpTimer > 0)
        {
            jumpTimer -= 1 * Time.deltaTime;
        }

        //AnimationStuff
        moveAnimator.SetFloat("Velocity", rb.velocity.x);
        moveAnimator.SetFloat("MoveDir", moveDir);
        moveAnimator.SetBool("IsGrounded", isGrounded);
    }

    void FixedUpdate()
    {
        //Patrolling stuff
       if  (!alerted && Home)
        {
            if (transform.position.x <= startX + minMaxPatrol.x)
            {
                if (waitTimer == 2)
                {
                    moveDir = 1;
                    speed = 0;
                }
                waitTimer -= 1 * Time.deltaTime;
                 if (waitTimer <= 0)
                {
                    speed = 1;
                    waitTimer = 2;
                }
            } else if (transform.position.x >= startX + minMaxPatrol.y)
            {
                if (waitTimer == 2)
                {
                    moveDir = -1;
                    speed = 0;
                }
                waitTimer -= 1 * Time.deltaTime;
                if (waitTimer <= 0)
                {
                    speed = 1;
                    waitTimer = 2;
                }
            }

        }

       //applys gravity if isGrounded
       if (!isGrounded)
        {
            rb.velocity += new Vector2(0, gravity);
        }

        //Actually moving
        if (!alerted)
        {
            rb.velocity = new Vector2(speed * moveDir, rb.velocity.y);
        }

        //Stops if abyss
        if (!Physics2D.Raycast(new Vector2(transform.position.x + 1f * moveDir, transform.position.y), new Vector2(0, -1), 4f, ground))
        {
            if (moveDir == 1 && rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else if (moveDir == -1 && rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }
}
