using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class HUDCameraConnector : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        // Alleen uitvoeren als de camera nog niet is ingesteld
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                canvas.worldCamera = mainCam;
                Debug.Log("HUD-camera gekoppeld aan: " + mainCam.name);
            }
            else
            {
                Debug.LogWarning("Geen Main Camera gevonden om aan HUD-canvas te koppelen.");
            }
        }
    }
}
