using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PotionSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image icon;
    public TextMeshProUGUI quantityText;

    private System.Action onClick;
    [HideInInspector] public Potion potion;

    public void Initialize(Potion newPotion, int quantity, System.Action clickCallback)
    {
        potion = newPotion;
        quantityText.text = quantity.ToString();
        onClick = clickCallback;

        if (icon != null)
        {
            if (newPotion.icon != null)
            {
                icon.sprite = newPotion.icon;
                icon.enabled = true;
            }
            else
            {
                icon.enabled = false;
                Debug.LogError($"❌ Potion '{newPotion.potionName}' has no icon!");
            }
        }
    }

    public void UpdateQuantity(int quantity)
    {
        quantityText.text = quantity.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameState.Diagnosing)
        {
            Debug.Log($"💊 Potion submitted: {potion.potionName}");
            CustomerManager.Instance.EvaluatePotion(potion);
        }
        else
        {
            onClick?.Invoke();
        }
    }
}