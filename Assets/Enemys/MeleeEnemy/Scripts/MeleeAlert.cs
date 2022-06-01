using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAlert : MonoBehaviour
{
    private EasyPathFinderScript eP;
    private Rigidbody2D rb;
    
    [SerializeField] private Transform player;
    [SerializeField] private float sprintSpeed = 3f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private SwordStuff sword;

    private float attackCooldown = 1;

    void Start()
    {
        eP = GetComponent<EasyPathFinderScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    
    void FixedUpdate()
    {
        if (eP.alerted)
        {
            //Sets Direction to move
            if (player.transform.position.x > transform.position.x && eP.moveDir != 1)
            {
                eP.moveDir = 1;
            } else if (player.transform.position.x < transform.position.x && eP.moveDir != -1)
            {
                eP.moveDir = -1;
            } 

            //Move in X
            rb.velocity = new Vector2(eP.moveDir * sprintSpeed, rb.velocity.y);

            //Attack
            if (Physics2D.Raycast(transform.position, new Vector2(eP.moveDir, 0), 1, playerLayer) && attackCooldown <= 0)
            {
                var Sword = Instantiate(sword, new Vector3(transform.position.x + 0.5f * eP.moveDir, transform.position.y - 0.4f, 0), Quaternion.identity);
                Sword.transform.parent = gameObject.transform;
                attackCooldown = 1;
            } else if (Physics2D.Raycast(transform.position, new Vector2(0, 1), 1, playerLayer) && attackCooldown <= 0)
            {
                var Sword = Instantiate(sword, new Vector3(transform.position.x, transform.position.y + 0.5f, 0), Quaternion.identity);
                Sword.transform.parent = gameObject.transform;
                attackCooldown = 1;
            } 


            //Stops if abyss
            if (!Physics2D.Raycast(new Vector2(transform.position.x + 0.5f * eP.moveDir, transform.position.y), new Vector2(0, -1), 3, ground))
            {
                if (eP.moveDir == 1 && rb.velocity.x > 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
                else if (eP.moveDir == -1 && rb.velocity.x < 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
    }
    private void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= 1 * Time.deltaTime;
        }
    }
}
