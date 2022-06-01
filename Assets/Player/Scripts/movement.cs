using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speed = -10f;
    public float gravity = 10f;
    public float jumpForce = 50f;
    public float maxVelocity = 10f;
    public float maxFallSpeed = 10f;
    public float grappingSpeed = 10f;
    public float cdTimer = 0.5f;
    public bool cd = false;
    public bool dead = false;


    bool aPressed;
    bool dPressed;
    bool shiftPress;

    private float movedir;


    bool stickR;
    bool stickL;
    bool grappling;
    bool slidn = true;


    public Grapplinghook gH;
    public GrapplingGun gG;
    public LayerMask ground;
    public GroundCheck groundC;
    public Transform player;
    public Animator moveAnimator;


    Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Cecks if buttons are being pressed
        aPressed = Input.GetKey(KeyCode.A);
        dPressed = Input.GetKey(KeyCode.D);
        shiftPress = Input.GetKey(KeyCode.LeftShift);

        if (cd)
        {
            if (cdTimer <= 0)
            {
                cd = false;
            } else
            {
                cdTimer -= 1 * Time.deltaTime;
            }
        }

        //Only executes if the player isnt grappled
        if (!gH.isGrappling)
        {
            //Checks if player just grappled and if it did
            if (!grappling) { grappling = true; }

            //Checks if no inputs are being made and makes the player slowly stop 
            if (!aPressed && !dPressed && !shiftPress && !cd)
            {
                rb.velocity = new Vector2(rb.velocity.x * 0.93f, rb.velocity.y);

                if ((rb.velocity.x < 0.1f && rb.velocity.x > 0) || (rb.velocity.x > -0.1f && rb.velocity.x < 0))
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }

            //Jump code
            if ((Input.GetKeyDown(KeyCode.Space) && groundC.isGrounded || Input.GetKeyDown(KeyCode.Space) && (stickR || stickL) ) && !dead)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce);
                if (slidn)
                {
                    rb.AddForce(Vector2.up * 10); ;
                }
                if (stickL)
                {
                    cd = true;
                    cdTimer = 0.5f;
                    rb.velocity = new Vector2(7, rb.velocity.y);
                } else if (stickR)
                {
                    cd = true;
                    cdTimer = 0.5f;
                    rb.velocity = new Vector2(-7, rb.velocity.y);
                }
            }
        }
        //Reset player position
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(-20, 2, transform.position.z);
            dead = false;
        }

        //Handles the animation variables
        if (rb.velocity.x > 0)
        {
            movedir = 1;
        } else if (rb.velocity.x < 0)
        {
            movedir = -1;
        }

        moveAnimator.SetFloat("Velocity", rb.velocity.x);
        moveAnimator.SetFloat("MoveDir", movedir);
        moveAnimator.SetBool("IsGrounded", groundC.isGrounded);
        moveAnimator.SetBool("IsSliding", !slidn);
        
    }

    void FixedUpdate()
    {
        //Only executes if the player isnt grappled
        if (!gH.isGrappling)
        {
            if ((!shiftPress || !groundC.isGrounded) && !dead)
            {
                //check if player just slided and transforms the player back to its original size
                if (!slidn)
                {
                    transform.localScale += new Vector3(-0.5f, 0.5f, 0);
                    slidn = true;
                }

                //Move left and right
                if (aPressed)
                {
                    rb.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
                }
                if (dPressed)
                {
                    rb.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
                }

                //Creates raycast to wallstick
                RaycastHit2D upLeft = Physics2D.Raycast(new Vector2(player.position.x - 0.25f, player.position.y + 0.45f), new Vector2(-1f, 0), 0.1f, ground);
                RaycastHit2D downLeft = Physics2D.Raycast(new Vector2(player.position.x - 0.25f, player.position.y - 0.45f), new Vector2(-1f, 0), 0.1f, ground);
                RaycastHit2D upRight = Physics2D.Raycast(new Vector2(player.position.x + 0.25f, player.position.y + 0.45f), new Vector2(1f, 0), 0.1f, ground);
                RaycastHit2D DownRight = Physics2D.Raycast(new Vector2(player.position.x + 0.25f, player.position.y - 0.45f), new Vector2(1f, 0), 0.1f, ground);

                if (upLeft.collider != null && downLeft.collider != null && groundC.isGrounded == false)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    stickL = true;
                }
                else if (upRight.collider != null && DownRight.collider != null && groundC.isGrounded == false)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    stickR = true;
                } else
                {
                    stickL = false;
                    stickR = false;
                }

                //Slidning
            } else if (!dead)
            {
                if (slidn)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 15);
                }

                if (groundC.isGrounded)
                {
                    rb.velocity = new Vector2(rb.velocity.x * 0.985f, rb.velocity.y);
                    if (slidn)
                    {
                        player.localScale += new Vector3(0.5f, -0.5f, 0);
                        slidn = false;
                    }
                }
            }

            //Caps speed
            rb.velocity = new Vector2(Mathf.Min(rb.velocity.x, maxVelocity), rb.velocity.y);
            rb.velocity = new Vector2(Mathf.Max(rb.velocity.x, -maxVelocity), Mathf.Min(rb.velocity.y, maxFallSpeed));

            //Applys gravity
            rb.velocity += new Vector2(0, gravity);

         //Movement when grappled 
        } else if (!dead)
        {
            //Checks if its the first update where the player is grappled and sets the grappled velocity according to position and velocity prior to being grappled
            if (grappling)
            {
                grappling = false;
                if (gG.grapplePoint.x < rb.position.x)
                {
                    grappingSpeed = rb.velocity.x + rb.velocity.y - 2f;
                }
                else if (gG.grapplePoint.x > rb.position.x)
                {
                    grappingSpeed = rb.velocity.x - rb.velocity.y + 2f;
                }
            }

            //Gives the player the accelerating and dingling feeling while grappling
            if (gG.grapplePoint.x < rb.position.x)
            {
                grappingSpeed += -0.2f;
                if (GetAngleFromVector(new Vector2(rb.position.x - gG.grapplePoint.x, rb.position.y - gG.grapplePoint.y)) >= 315 && rb.velocity.x >= 0)
                {
                    grappingSpeed -= 0.4f;
                }
            } else if (gG.grapplePoint.x > rb.position.x)
            {
                grappingSpeed += 0.2f;
                if (GetAngleFromVector(new Vector2(rb.position.x - gG.grapplePoint.x, rb.position.y - gG.grapplePoint.y)) <= 225 && rb.velocity.x <= 0)
                {
                    grappingSpeed += 0.4f;
                }
            }

            //applys the forces to the velocity
            rb.velocity = new Vector2(grappingSpeed, 0);
        }

        

    }

    public void Death()
    {
        dead = true;
    }

    //Funktion that converts vector3 to angle
    public float GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
