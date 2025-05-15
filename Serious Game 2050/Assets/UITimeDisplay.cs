using UnityEngine;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text uiText;
    [SerializeField] private UITimeManager timeManager;


    void Update()
    {
        float seconds = timeManager.CurrentGameTime;
        uiText.text = ConvertTime(seconds);
    }

    string ConvertTime(float seconds)
    {
        float timePerDay = timeManager.GetTimePerDay();
        float timePerHour = timePerDay / 24f;
        float timePerMinute = timePerHour / 60f;

        int day = Mathf.FloorToInt(seconds / timePerDay) + 1;
        float timeIntoDay = seconds % timePerDay;
        int hour = Mathf.FloorToInt(timeIntoDay / timePerHour);
        int minute = Mathf.FloorToInt((timeIntoDay % timePerHour) / timePerMinute);

        return $"Dag {day}, {hour:00}:{minute:00} uur";
    }
}
