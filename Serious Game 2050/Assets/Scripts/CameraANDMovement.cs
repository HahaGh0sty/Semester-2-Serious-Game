using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(PixelPerfectCamera))]
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float zoomSpeed = 4f;
    public float minZoom = 1f;
    public float maxZoom = 100f;
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
        if (ppc != null) ppc.upscaleRT = false; // Allow zoom
    }

    void Update()
    {
        float currentMoveSpeed = mainCamera.orthographicSize < 4f ? 7f : moveSpeed;
        if (isSpeedDoubled) currentMoveSpeed *= 3f;

        Vector3 targetPosition = transform.position;

        // WASD Movement
        if (Input.GetKey(KeyCode.W)) targetPosition.y += currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) targetPosition.y -= currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) targetPosition.x -= currentMoveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) targetPosition.x += currentMoveSpeed * Time.deltaTime;

        // Right-click drag
        if (Input.GetMouseButtonDown(1))
            lastMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            targetPosition -= new Vector3(delta.x, delta.y, 0f) * mainCamera.orthographicSize * 0.002f;
            lastMousePosition = Input.mousePosition;
        }

        // Simplified Zoom (no offset)
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float keyboardZoom = 0f;
        if (Input.GetKey(KeyCode.LeftShift)) keyboardZoom -= 1f;
        if (Input.GetKey(KeyCode.LeftControl)) keyboardZoom += 1f;

        float zoomDelta = scrollInput + keyboardZoom;
        if (zoomDelta != 0f)
        {
            float targetZoom = mainCamera.orthographicSize - (zoomDelta * zoomSpeed * 10f);
            mainCamera.orthographicSize = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        // Smooth follow
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (Input.GetKeyDown(KeyCode.F))
            isSpeedDoubled = !isSpeedDoubled;
    }
}
