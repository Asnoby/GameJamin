using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordStuff : MonoBehaviour
{

    [SerializeField] private LayerMask player;

    private float timer;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        if (timer <= 0.25)
        {
            transform.localPosition += new Vector3(0, 3.2f, 0) * Time.deltaTime;
            timer += 1 * Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.IsTouchingLayers(player))
        {
            try
            {
                c.gameObject.GetComponent<movement>().Death();
            }
            catch { }
        }
    }
}
