using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraDetection : MonoBehaviour
{
    [SerializeField] private Transform pfFov;
    [SerializeField] private float fov = 90f;
    [SerializeField] private float viewDistance = 5f;
    [SerializeField] private Transform player;
    [SerializeField] private Material calm;
    [SerializeField] private Material angry;
    [SerializeField] private LayerMask hittable;
    [SerializeField] private float speed = 5;
    [SerializeField] private Vector2 minMaxDegrees = new Vector2 (90, 180);
    [SerializeField] private UnityEvent Alert;
    private bool up;
    private MeshRenderer current;
    public bool alert = false;
    private FieldOfView fieldOfView;
    private Vector3 aimDir;



    private void Start()
    {
        fieldOfView = Instantiate(pfFov, null).GetComponent<FieldOfView>();
        fieldOfView.PfieldOfView = fov;
        fieldOfView.PviewDistance = viewDistance;
        aimDir = fieldOfView.GetVectorFromAngle(transform.rotation.eulerAngles.z + 45);
        current = fieldOfView.GetComponent<MeshRenderer>();
}
    void Update()
    {
        aimDir = fieldOfView.GetVectorFromAngle(transform.rotation.eulerAngles.z + Mathf.Cos(180));
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDir(aimDir);
        FindTargetPlayer();
    }

    private void FixedUpdate()
    {
        if (transform.localEulerAngles.z <= minMaxDegrees.x )
        {
            up = true;
        } else if (transform.localEulerAngles.z >= minMaxDegrees.y)
        {
            up = false;
        }
        if (up)
        {
            transform.eulerAngles += new Vector3(0, 0, speed * Time.deltaTime);
        } else if (!up)
        {
            transform.eulerAngles -= new Vector3(0, 0, speed * Time.deltaTime);
        }

    }

    private void FindTargetPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(aimDir, dirToPlayer) < fov / 2)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer, viewDistance, hittable);
                if(hit.collider.gameObject.layer == 7)
                {
                    if (!alert)
                    {
                        current.material = angry;
                        alert = true;
                        Alert.Invoke();
                    }
                } 
                else if (alert)
                {
                    current.material = calm;
                    alert = false;
                }
            }
            else if (alert)
            {
                current.material = calm;
                alert = false;
            }
        }
        else if (alert)
        {
            current.material = calm;
            alert = false;
        }
    }
}
