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
    private Vector3 lastMousePosition;

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

        // Smooth movement with WASD keys
        Vector3 targetPosition = transform.position;

        if (Input.GetKey(KeyCode.W)) targetPosition.y += moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) targetPosition.y -= moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) targetPosition.x -= moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) targetPosition.x += moveSpeed * Time.deltaTime;

        // Right-click dragging to move the map
        if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1)) // Right mouse button held
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            targetPosition -= new Vector3(delta.x, delta.y, 0) * cam.orthographicSize * 0.002f; // Adjust sensitivity
            lastMousePosition = Input.mousePosition;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Zoom control
        float targetZoom = cam.orthographicSize;

        if (Input.GetKey(KeyCode.LeftShift)) targetZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) targetZoom += zoomSpeed * Time.deltaTime;

        // Add scroll wheel zooming
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            targetZoom -= scrollInput * zoomSpeed * 2f;
        }

        cam.orthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        // Toggle speed when "F" is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            isSpeedDoubled = !isSpeedDoubled;  // Toggle the speed
        }
    }
}
