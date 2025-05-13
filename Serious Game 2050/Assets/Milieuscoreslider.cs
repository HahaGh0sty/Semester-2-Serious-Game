using UnityEngine;
using UnityEngine.UI;

public class MilieuScoreKeyboard : MonoBehaviour
{
    public Slider milieuSlider;
    public Image fillImage; // verwijzing naar de fill van de slider
    public int currentScore = 50;
    public int minScore = 0;
    public int maxScore = 100;

    void Start()
    {
        UpdateSlider();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            ChangeScore(+10);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeScore(-10);
        }
    }

    void ChangeScore(int delta)
    {
        currentScore += delta;
        currentScore = Mathf.Clamp(currentScore, minScore, maxScore);
        UpdateSlider();
    }

    void UpdateSlider()
    {
        float normalized = (float)currentScore / maxScore;
        milieuSlider.value = normalized;

        // Kleur aanpassen op basis van score
        if (currentScore >= 70)
        {
            fillImage.color = Color.green;
        }
        else if (currentScore >= 30)
        {
            fillImage.color = new Color(1f, 0.64f, 0f); // oranje
        }
        else
        {
            fillImage.color = Color.red;
        }
    }
}
