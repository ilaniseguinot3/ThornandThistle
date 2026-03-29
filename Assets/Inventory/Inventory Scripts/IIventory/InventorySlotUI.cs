using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [Header("UI References")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI quantityText;
    public Button button;

    [Header("Visual Settings")]
    public Color normalColor = Color.white;
    public Color disabledColor = new Color(1f, 1f, 1f, 0.35f); // faded white
    public float disabledOpacity = 0.35f;

    private Ingredient ingredientData;
    private int quantity;

    public void Initialize(Ingredient ingredient, int quantity, System.Action<Ingredient> onClickAction)
    {
        ingredientData = ingredient;
        this.quantity = quantity;

        // Assign visuals
        if (iconImage) iconImage.sprite = ingredient.icon;
        if (nameText) nameText.text = ingredient.ingredientName;
        if (quantityText) quantityText.text = "x" + quantity;

        // Clear previous listeners
        button.onClick.RemoveAllListeners();

        // Add click listener
        button.onClick.AddListener(() => onClickAction?.Invoke(ingredientData));

        // Update appearance
        UpdateVisualState();
    }

    public void UpdateQuantity(int newQuantity)
    {
        quantity = newQuantity;
        if (quantityText) quantityText.text = "x" + quantity;
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        bool isDepleted = quantity <= 0;

        // Gray out & fade if empty
        if (iconImage)
        {
            iconImage.color = isDepleted ? disabledColor : normalColor;
        }

        if (nameText)
        {
            Color textColor = nameText.color;
            textColor.a = isDepleted ? disabledOpacity : 1f;
            nameText.color = textColor;
        }

        if (quantityText)
        {
            Color textColor = quantityText.color;
            textColor.a = isDepleted ? disabledOpacity : 1f;
            quantityText.color = textColor;
        }

        // Disable button interaction when empty
        if (button)
            button.interactable = !isDepleted;
    }
}
