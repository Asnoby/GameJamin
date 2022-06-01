using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    public bool isGrounded;
    public LayerMask ground;

    void Update()
    {
        isGrounded = Physics2D.OverlapBox(transform.position, new Vector2(0.2f, 0.6f), 0, ground);
    }
}
