using UnityEngine;

public class UITimeManager : MonoBehaviour
{
    public float CurrentGameTime { get; private set; } = 0f;

    [SerializeField] private float timePerDay = 600f; // 10 min per dag

    void Update()
    {
        CurrentGameTime += Time.deltaTime;
    }

    public float GetTimePerDay()
    {
        return timePerDay;
    }
}
