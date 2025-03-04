using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float zoomSpeed = 4f;
    public float minZoom = 1f;
    public float maxZoom = 1000f;
    public float smoothTime = 0.01f;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;
    private bool isSpeedDoubled = false;  // Track if speed is doubled

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Adjust move speed based on camera size
        if (cam.orthographicSize < 4)
        {
            moveSpeed = 7f;
        }
        else
        {
            moveSpeed = 20f;
        }

        // Apply the speed doubling effect from the "F" key
        if (isSpeedDoubled)
        {
            moveSpeed *= 3f;
        }

        // Smooth movement
        Vector3 targetPosition = transform.position;

        if (Input.GetKey(KeyCode.W)) targetPosition.y += moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) targetPosition.y -= moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) targetPosition.x -= moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) targetPosition.x += moveSpeed * Time.deltaTime;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Zoom control
        float targetZoom = cam.orthographicSize;

        if (Input.GetKey(KeyCode.LeftShift)) targetZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) targetZoom += zoomSpeed * Time.deltaTime;

        cam.orthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        // Toggle speed when "F" is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            isSpeedDoubled = !isSpeedDoubled;  // Toggle the speed
        }
    }
}
