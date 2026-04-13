using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool CanCraft(Recipe recipe)
    {
        foreach (var req in recipe.ingredients)
        {
            if (InventoryManager.Instance.GetIngredientQuantity(req.ingredient) < req.requiredAmount)
                return false;
        }
        return true;
    }

    public void Craft(Recipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log($"❌ Not enough ingredients to craft {recipe.resultPotion.potionName}");
            return;
        }

        // Calculate total ingredient cost
        int totalCost = 0;
        foreach (var req in recipe.ingredients)
            totalCost += req.ingredient.cost * req.requiredAmount;

        // Attempt to spend — aborts if player is too poor
        if (totalCost > 0 && !MoneyManager.Instance.TrySpend(totalCost))
        {
            Debug.Log($"❌ Can't afford to brew {recipe.resultPotion.potionName} (costs {totalCost}g)");
            return;
        }

        // Consume ingredients
        foreach (var req in recipe.ingredients)
        {
            InventoryManager.Instance.RemoveIngredient(req.ingredient, req.requiredAmount);
            Debug.Log($"🧂 Used {req.requiredAmount}x {req.ingredient.ingredientName}");
        }

        InventoryManager.Instance.AddPotion(recipe.resultPotion, recipe.resultAmount);
        InventoryEvents.OnInventoryChanged.Invoke();
        Debug.Log($"🍾 Crafted {recipe.resultAmount}x {recipe.resultPotion.potionName} for {totalCost}g");
    }
}