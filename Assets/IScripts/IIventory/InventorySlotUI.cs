using UnityEngine;
using UnityEngine.UI;
using TMPro; // Optional, if you’re using TextMeshPro for UI text

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI nameText;      // use Text if you’re not using TMP
    public TextMeshProUGUI quantityText;  // use Text if you’re not using TMP
    public Button button;

    private Ingredient ingredientData;
    private int quantity;

    // Called by InventoryUIManager when creating slots
    public void Initialize(Ingredient ingredient, int quantity, System.Action<Ingredient> onClickAction)
    {
        ingredientData = ingredient;
        this.quantity = quantity;

        // Assign visuals
        if (iconImage) iconImage.sprite = ingredient.icon;
        if (nameText) nameText.text = ingredient.ingredientName;
        if (quantityText) quantityText.text = "x" + quantity;

        // Clear previous listeners to avoid duplicates
        button.onClick.RemoveAllListeners();

        // Add a new listener that passes this ingredient to the InventoryUIManager callback
        button.onClick.AddListener(() => onClickAction?.Invoke(ingredientData));
    }

    // Optional helper if you want to update just the quantity later
    public void UpdateQuantity(int newQuantity)
    {
        quantity = newQuantity;
        if (quantityText) quantityText.text = "x" + quantity;
    }
}
