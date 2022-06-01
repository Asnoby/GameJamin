using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlock : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private movement PlayerScript;

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.IsTouchingLayers(player))
        {
            try
            {
                PlayerScript.Death();
            }
            catch { }
        }
    }
}
