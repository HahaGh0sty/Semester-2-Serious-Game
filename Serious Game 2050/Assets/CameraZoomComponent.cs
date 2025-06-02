using UnityEngine;
using UnityEngine.U2D;

public class CameraZoomButtons : MonoBehaviour
{
    public PixelPerfectCamera pixelPerfectCamera;

    private Vector2Int[] resolutions = new Vector2Int[]
    {
        new Vector2Int(64, 36),
        new Vector2Int(128, 72),
        new Vector2Int(192, 108),
        new Vector2Int(256, 144),
        new Vector2Int(320, 180), // Default
        new Vector2Int(640, 360),
        new Vector2Int(1280, 720),
    };

    private int currentZoomIndex = 4; // Start fully zoomed out

    void Start()
    {
        if (pixelPerfectCamera == null)
        {
            pixelPerfectCamera = Camera.main.GetComponent<PixelPerfectCamera>();
        }

        ApplyZoom();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ZoomIn();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ZoomOut();
        }
    }

    public void ZoomIn()
    {
        Debug.Log("Zoom In");
        if (currentZoomIndex > 0)
        {
            currentZoomIndex--;
            ApplyZoom();
        }
    }

    public void ZoomOut()
    {
        Debug.Log("Zoom Out");
        if (currentZoomIndex < resolutions.Length - 1)
        {
            currentZoomIndex++;
            ApplyZoom();
        }
    }

    private void ApplyZoom()
    {
        Vector2Int res = resolutions[currentZoomIndex];
        pixelPerfectCamera.refResolutionX = res.x;
        pixelPerfectCamera.refResolutionY = res.y;

        // Force PPC to recalculate
        pixelPerfectCamera.enabled = false;
        pixelPerfectCamera.enabled = true;

        Debug.Log($"Zoom applied: {res.x}x{res.y}");
    }
}
