using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text uiText;
    public TimeManager timeManager;


    void Update()
    {
        uiText.text = timeManager.CurrentYear.ToString();
        
    }
}

