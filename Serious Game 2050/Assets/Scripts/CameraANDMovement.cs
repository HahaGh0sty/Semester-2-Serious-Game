using UnityEngine;
using UnityEngine.U2D; // Required for PixelPerfectCamera

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PixelPerfectCamera))]
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float zoomSpeed = 4f;
    public float minZoom = 1f;
    public float maxZoom = 1000f;
    public float smoothTime = 0.01f;

    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;
    private PixelPerfectCamera ppc;
    private bool isSpeedDoubled = false;
    private Vector3 lastMousePosition;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        ppc = GetComponent<PixelPerfectCamera>();
    }

    void Update()
    {
        if (Time.timeScale == 0) return;

        float currentMoveSpeed = mainCamera.orthographicSize < 4f ? 7f : moveSpeed;
        if (isSpeedDoubled)
        {
            currentMoveSpeed *= 3f;
        }

        Vector3 targetPosition = transform.position;

        // WASD Movement
        if (Input.GetKey(KeyCode.W)) targetPosition.y += currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) targetPosition.y -= currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) targetPosition.x -= currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) targetPosition.x += currentMoveSpeed * Time.deltaTime;

        // Right-click drag movement
        if (Input.GetMouseButtonDown(1))
        {
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            targetPosition -= new Vector3(delta.x, delta.y, 0f) * mainCamera.orthographicSize * 0.002f;
            lastMousePosition = Input.mousePosition;
        }

        // Zoom input from scroll and keys
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float keyboardZoom = 0f;

        if (Input.GetKey(KeyCode.LeftShift)) keyboardZoom -= 1f;
        if (Input.GetKey(KeyCode.LeftControl)) keyboardZoom += 1f;

        float combinedZoomInput = scrollInput + keyboardZoom;

        if (combinedZoomInput != 0f)
        {
            Vector3 mouseWorldBeforeZoom = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            float targetZoom = mainCamera.orthographicSize - (combinedZoomInput * zoomSpeed * 10f);
            mainCamera.orthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);

            Vector3 mouseWorldAfterZoom = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 zoomOffset = mouseWorldBeforeZoom - mouseWorldAfterZoom;
            targetPosition += zoomOffset;

            // Toggle Pixel Perfect Camera only at native zoom
            if (ppc != null)
            {
                float nativeZoom = ppc.refResolutionY / (2f * (float)ppc.assetsPPU);
                ppc.enabled = Mathf.Abs(mainCamera.orthographicSize - nativeZoom) < 0.1f;
            }
        }

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // Speed toggle with F key
        if (Input.GetKeyDown(KeyCode.F))
        {
            isSpeedDoubled = !isSpeedDoubled;
        }
    }
}
