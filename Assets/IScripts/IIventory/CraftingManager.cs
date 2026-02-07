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

    public bool CanCraft(Recipe recipe)
    {
        foreach (var req in recipe.ingredients)
        {
            int owned = InventoryManager.Instance.GetQuantity(req.ingredient);
            if (owned < req.requiredAmount)
                return false;
        }
        return true;
    }

    public void Craft(Recipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("Not enough ingredients to craft " + recipe.resultPotion.potionName);
            return;
        }

        // Consume ingredients
        foreach (var req in recipe.ingredients)
        {
            InventoryManager.Instance.RemoveItem(req.ingredient, req.requiredAmount);
        }

        // Add potion as new item (you can track potions in same inventory)
        Ingredient potionAsIngredient = ScriptableObject.CreateInstance<Ingredient>();
        potionAsIngredient.ingredientName = recipe.resultPotion.potionName;
        InventoryManager.Instance.AddItem(potionAsIngredient, recipe.resultAmount);

        Debug.Log($"Crafted {recipe.resultAmount}x {recipe.resultPotion.potionName}");
    }
}
