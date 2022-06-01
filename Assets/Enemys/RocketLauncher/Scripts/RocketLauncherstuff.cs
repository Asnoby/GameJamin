using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class RocketLauncherstuff : MonoBehaviour
{
    public AIDestinationSetter Rocket;
    public Transform player;
    public Transform fakeRocket;
    public float coolDown = 4f;
    public CameraDetection cD;
    private Transform tempRocket;
    private bool ejecting = false;
    private float countDown = 2f;
    
    

    void Update()
    {
        if (coolDown > 0)
        coolDown -= 1 * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (ejecting)
        {
            if (countDown <= 0)
            {
                Destroy(tempRocket.gameObject);
                SpawnRocket();
                ejecting = false;
            }
            else
            {
                tempRocket.position += new Vector3(0, 0.3f *Time.deltaTime, 0);
                countDown -= 1 * Time.deltaTime;
            }
        }
    }

    public void StartEjecting()
    {
        if (coolDown <= 0)
        {
            countDown = 2;
            tempRocket = Instantiate(fakeRocket, new Vector3(transform.position.x, transform.position.y - 0.6f, transform.position.z), Quaternion.identity);
            ejecting = true;
            coolDown = 4f;
        }
    }

    void SpawnRocket()
    {
        Rocket.target = player;
        Instantiate(Rocket, transform.position, Quaternion.identity);
    }

    /*set target.missle:
    follow arvid.player = true if:
        launch = true
    Oh praise the all mighty lord Sam/Jesus*/
}
