using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PotionSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    public Image icon;                  // Assign in prefab Inspector
    public TextMeshProUGUI quantityText;

    private System.Action onClick;
    [HideInInspector] public Potion potion;

    public void Initialize(Potion newPotion, int quantity, System.Action clickCallback)
    {
        potion = newPotion;
        quantityText.text = quantity.ToString();
        onClick = clickCallback;

        // ✅ Properly assign the sprite to the Image component
        if (icon != null)
        {
            if (newPotion.icon != null)
            {
                icon.sprite = newPotion.icon;
                icon.enabled = true;
                Debug.Log($"✅ Potion icon assigned: {newPotion.icon.name}");
            }
            else
            {
                icon.enabled = false; // hide broken image rather than show blank
                Debug.LogError($"❌ Potion '{newPotion.potionName}' has no icon assigned in its ScriptableObject!");
            }
        }
        else
        {
            Debug.LogError($"❌ PotionSlotUI on '{gameObject.name}' has no Image component assigned for 'icon'!");
        }
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