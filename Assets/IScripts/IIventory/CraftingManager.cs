using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Checks if the player has enough ingredients to craft the given recipe.
    /// </summary>
    public bool CanCraft(Recipe recipe)
    {
        foreach (var req in recipe.ingredients)
        {
            int owned = InventoryManager.Instance.GetIngredientQuantity(req.ingredient);
            if (owned < req.requiredAmount)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Attempts to craft the recipe — consumes ingredients and adds the resulting potion.
    /// </summary>
    public void Craft(Recipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log($"❌ Not enough ingredients to craft {recipe.resultPotion.potionName}");
            return;
        }

        // Consume required ingredients
        foreach (var req in recipe.ingredients)
        {
            InventoryManager.Instance.RemoveIngredient(req.ingredient, req.requiredAmount);
            Debug.Log($"🧂 Used {req.requiredAmount}x {req.ingredient.ingredientName}");
        }

        // Add resulting potion(s)
        InventoryManager.Instance.AddPotion(recipe.resultPotion, recipe.resultAmount);
        InventoryEvents.OnInventoryChanged.Invoke(); // updates the UI
        Debug.Log($"🍾 Crafted {recipe.resultAmount}x {recipe.resultPotion.potionName}");
    }
}
