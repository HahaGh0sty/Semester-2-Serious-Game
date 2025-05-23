using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float minZoom = 1f;
    public float maxZoom = 50f;
    public float smoothTime = 0.02f;
    public float[] zoomLevels = { 1f, 5f, 12f, 25f, 50f };
    public float[] zPositions = { -5f, -10f, -15f, -20f, -25f };
    private int currentZoomIndex = 2; // Start at zoom level 12
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private bool isSpeedDoubled = false;  // Track if speed is doubled
    private Vector3 lastMousePosition;

    public RenderCameraManager tileLoader; // assign this in the Inspector

    private float GetZFromOrthoSize(float orthoSize)
    {
        if (orthoSize >= 25f)
            return -30f;
        else if (orthoSize >= 12f)
            return -20f;
        else if (orthoSize >= 5f)
            return -15f;
        else // zoomed in very close
            return -10f;
    }


    void Start()
    {
        cam = GetComponent<Camera>();

        if (zoomLevels.Length != zPositions.Length)
        {
            Debug.LogError("zoomLevels and zPositions must have the same length!");
            return;
        }

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

        // PRELOAD tiles around where we're about to move to
        if (tileLoader != null) tileLoader.PreloadTilesAt(targetPosition);

        // Preserve current Z from transform.position
        targetPosition.z = transform.position.z;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.F)) isSpeedDoubled = !isSpeedDoubled;
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


        Debug.Log($"Zoom applied: {cam.orthographicSize}, Z: {cam.transform.position.z}");
    }


}
