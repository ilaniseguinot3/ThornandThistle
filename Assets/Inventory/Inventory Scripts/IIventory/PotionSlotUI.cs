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
                Debug.LogError($"❌ Potion '{newPotion.potionName}' has no icon assigned!");
            }
        }
    }

    public void UpdateQuantity(int quantity)
    {
        quantityText.text = quantity.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Only allow potion selection during diagnosis mode
        if (GameState.Diagnosing)
        {
            Debug.Log($"💊 Potion selected during diagnosis: {potion.potionName}");
            CustomerManager.Instance.EvaluatePotion(potion);
        }
        else
        {
            // Normal click behaviour
            onClick?.Invoke();
        }
    }
}