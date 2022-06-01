using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketStuff : MonoBehaviour
{

    public LayerMask player;
    public LayerMask ground;

    public float timer = 10;

    private void FixedUpdate()
    {
        if (timer >= 0)
        {
            timer -= 1f * Time.deltaTime;
        } else
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.IsTouchingLayers(player) || !c.IsTouchingLayers(ground))
        {
            try
            {
                c.gameObject.GetComponent<movement>().Death();
            }
            catch { }
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
    }
}
