using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private float doorSpeed = 0.2f;
    [SerializeField] private Transform door;

    private float startY;
    bool open = false;

    private void Start()
    {
        startY = door.transform.position.y;
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.IsTouchingLayers(player))
        {
            open = true;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (!c.IsTouchingLayers(player))
        {
            open = false;
        }
    }

    private void Update()
    {
        if (open && startY + 3 > door.transform.position.y)
        {
            door.transform.position += new Vector3(0, doorSpeed, 0);
        } else if (!open && startY < door.transform.position.y)
        {
            door.transform.position -= new Vector3(0, doorSpeed, 0);
        }
    }


}
