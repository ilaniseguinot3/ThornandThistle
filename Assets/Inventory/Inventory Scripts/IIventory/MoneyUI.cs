using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI goldText;

    private void OnEnable()
    {
        MoneyManager.OnMoneyChanged.AddListener(UpdateDisplay);
    }

    private void OnDisable()
    {
        MoneyManager.OnMoneyChanged.RemoveListener(UpdateDisplay);
    }

    private void Start() => UpdateDisplay();

    private void UpdateDisplay()
    {
        if (goldText != null)
            goldText.text = $"💰 {MoneyManager.Instance.CurrentGold}g";
    }
}