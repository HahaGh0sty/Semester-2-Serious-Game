using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDLoader : MonoBehaviour
{
    void Start()
    {
        // Controleer of de HUDScene al geladen is
        if (!SceneManager.GetSceneByName("HUD").isLoaded)
        {
            SceneManager.LoadScene("HUD", LoadSceneMode.Additive);
        }
    }
}
