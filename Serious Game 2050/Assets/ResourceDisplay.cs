using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceDisplay : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;

    public void UpdateDisplay(Sprite sprite, int amount)
    {
        icon.sprite = sprite;
        amountText.text = amount.ToString();
    }
}
