using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 Offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private float ScrollSpeed = 10;
    [SerializeField] private Camera ZoomCamera;

    [SerializeField] private Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        ZoomCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = Target.position + Offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            //wheel goes up
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            //wheel goes down
        }
    }

    
}