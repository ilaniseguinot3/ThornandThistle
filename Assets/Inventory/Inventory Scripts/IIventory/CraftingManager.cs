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

    // CraftingManager.cs — replace Craft() with this
    public bool Craft(Recipe recipe)
    {
        int totalCost = 0;
        foreach (var req in recipe.ingredients)
            totalCost += req.ingredient.cost * req.requiredAmount;

        if (totalCost > 0 && !MoneyManager.Instance.TrySpend(totalCost))
        {
            Debug.Log($"❌ Can't afford to brew {recipe.resultPotion.potionName} (costs {totalCost}g)");
            return false;
        }

        // ✅ Don't re-remove from inventory — already removed when added to cauldron
        InventoryManager.Instance.AddPotion(recipe.resultPotion, recipe.resultAmount);
        InventoryEvents.OnInventoryChanged.Invoke();
        Debug.Log($"🍾 Crafted {recipe.resultAmount}x {recipe.resultPotion.potionName} for {totalCost}g");
        return true;
    }
}