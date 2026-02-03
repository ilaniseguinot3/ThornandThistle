using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemUI : MonoBehaviour
{
    [Header("UI References")]
    public Image itemIcon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI itemTypeText;
    public TextMeshProUGUI slotNumberText;
    public Button removeButton;

    private InventoryItem currentItem;
    private int itemIndex;

    private void Start()
    {
        if (removeButton != null)
        {
            removeButton.onClick.AddListener(OnRemoveClicked);
        }
    }

    public void SetupItem(InventoryItem item, int index)
    {
        currentItem = item;
        itemIndex = index;

        // Set icon
        if (itemIcon != null && item.itemIcon != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.enabled = true;
        }
        else if (itemIcon != null)
        {
            itemIcon.enabled = false;
        }

        // Set name
        if (itemNameText != null)
        {
            itemNameText.text = item.itemName;
        }

        // Set description
        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = item.itemDescription;
        }

        // Set type
        if (itemTypeText != null)
        {
            itemTypeText.text = $"Type: {item.ingredientType}";
            
            // Optional: Color code by type
            itemTypeText.color = GetTypeColor(item.ingredientType);
        }

        // Set slot number
        if (slotNumberText != null)
        {
            slotNumberText.text = $"Slot #{index + 1}";
        }
    }

    private Color GetTypeColor(IngredientType type)
    {
        switch (type)
        {
            case IngredientType.Fire:
                return new Color(1f, 0.3f, 0.3f); // Red
            case IngredientType.Water:
                return new Color(0.3f, 0.5f, 1f); // Blue
            case IngredientType.Earth:
                return new Color(0.6f, 0.4f, 0.2f); // Brown
            case IngredientType.Wind:
                return new Color(0.8f, 1f, 0.8f); // Light green
            case IngredientType.Lightning:
                return new Color(1f, 1f, 0.3f); // Yellow
            case IngredientType.Nature:
                return new Color(0.2f, 0.8f, 0.2f); // Green
            case IngredientType.Ice:
                return new Color(0.7f, 0.9f, 1f); // Cyan
            case IngredientType.Dark:
                return new Color(0.4f, 0.2f, 0.5f); // Purple
            case IngredientType.Light:
                return new Color(1f, 1f, 0.9f); // White
            case IngredientType.Poison:
                return new Color(0.5f, 0.2f, 0.8f); // Purple
            default:
                return Color.white;
        }
    }

    private void OnRemoveClicked()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItemAt(itemIndex);
        }
    }

    private void OnDestroy()
    {
        if (removeButton != null)
        {
            removeButton.onClick.RemoveListener(OnRemoveClicked);
        }
    }
}