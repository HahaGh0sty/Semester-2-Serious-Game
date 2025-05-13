using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MapSizeSelector : MonoBehaviour
{
    public TMP_InputField widthInput;
    public TMP_InputField heightInput;

    public void StartGame()
    {
        int width = 50;
        int height = 50;

        int.TryParse(widthInput.text, out width);
        int.TryParse(heightInput.text, out height);

        GameSettings.MapWidth = width;
        GameSettings.MapHeight = height;

        Debug.Log("Map size selected: " + width + "x" + height);

        SceneManager.LoadScene("Luc's Scene"); // or whatever your scene is called
    }

}
