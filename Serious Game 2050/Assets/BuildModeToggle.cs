using UnityEngine;
using UnityEngine.UI;

public class BuildButtonHotkey : MonoBehaviour
{
    private Button buildButton;

    void Start()
    {
        buildButton = GetComponent<Button>();
        if (buildButton == null)
        {
            Debug.LogError("BuildButtonHotkey requires a Button component on the same GameObject.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && buildButton != null)
        {
            buildButton.onClick.Invoke();
        }
    }
}
