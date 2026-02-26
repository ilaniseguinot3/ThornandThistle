using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PotionSlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TextMeshProUGUI quantityText;


    private System.Action onClick;
    [HideInInspector] public Potion potion;

    public void Initialize(Potion newPotion, int quantity, System.Action clickCallback)
    {
        potion = newPotion;
        icon.sprite = potion.icon;
        // Add this temporarily to PotionSlotUI.Initialize() to debug:
        if (potion.icon == null)
            Debug.LogError($"❌ Potion '{potion.potionName}' has no icon assigned!");
        else
            Debug.Log($"✅ Potion icon found: {potion.icon.name}");
        quantityText.text = quantity.ToString();
        onClick = clickCallback;
    }

    public void UpdateQuantity(int quantity)
    {
        quantityText.text = quantity.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
