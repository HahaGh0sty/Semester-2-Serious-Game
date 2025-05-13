using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float minZoom = 1f;
    public float maxZoom = 50f;
    public float smoothTime = 0.02f;
    public float[] zoomLevels = { 1f, 5f, 12f, 25f, 50f };
    private int currentZoomIndex = 2; // Start at zoom level 12
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private bool isSpeedDoubled = false;  // Track if speed is doubled
    private Vector3 lastMousePosition;

    void Start()
    {
        cam = GetComponent<Camera>();
        ApplyZoom();
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
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            targetPosition -= new Vector3(delta.x, delta.y, 0) * cam.orthographicSize * 0.002f;
            lastMousePosition = Input.mousePosition;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Toggle speed when "F" is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            isSpeedDoubled = !isSpeedDoubled;
        }
    }

    public void ZoomIn()
    {
        Debug.Log("Zoom In button clicked.");
        if (currentZoomIndex > 0)
        {
            currentZoomIndex--;
            ApplyZoom();
        }
    }

    public void ZoomOut()
    {
        Debug.Log("Zoom Out button clicked.");
        if (currentZoomIndex < zoomLevels.Length - 1)
        {
            currentZoomIndex++;
            ApplyZoom();
        }
    }

    public void ApplyZoom()
    {
        cam.orthographicSize = zoomLevels[currentZoomIndex];
        Debug.Log("Zoom applied: " + cam.orthographicSize);
    }


}
