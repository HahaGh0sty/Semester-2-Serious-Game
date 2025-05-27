using UnityEngine;

public class HUDPersistent : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Optioneel: voorkom duplicaten bij herladen
        if (FindObjectsOfType<HUDPersistent>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
