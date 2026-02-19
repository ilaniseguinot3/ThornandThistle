using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private Dictionary<Ingredient, InventoryItem> ingredientInventory = new();
    private Dictionary<Potion, InventoryItem> potionInventory = new();

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

    // ================= Ingredients =================
    public void AddIngredient(Ingredient ingredient, int amount)
    {
        if (ingredientInventory.ContainsKey(ingredient))
            ingredientInventory[ingredient].quantity += amount;
        else
            ingredientInventory.Add(ingredient, new InventoryItem(ingredient, amount));

        InventoryEvents.OnInventoryChanged.Invoke();
    }

    public void RemoveIngredient(Ingredient ingredient, int amount)
    {
        if (ingredientInventory.ContainsKey(ingredient))
        {
            ingredientInventory[ingredient].quantity -= amount;
            if (ingredientInventory[ingredient].quantity <= 0)
                ingredientInventory.Remove(ingredient);
        }

        InventoryEvents.OnInventoryChanged.Invoke();
    }

    public List<InventoryItem> GetAllIngredients()
    {
        return new List<InventoryItem>(ingredientInventory.Values);
    }

    public int GetIngredientQuantity(Ingredient ingredient)
    {
        return ingredientInventory.ContainsKey(ingredient) ? ingredientInventory[ingredient].quantity : 0;
    }

    // ================= Potions =================
    public void AddPotion(Potion potion, int amount)
    {
        if (potionInventory.ContainsKey(potion))
            potionInventory[potion].quantity += amount;
        else
            potionInventory.Add(potion, new InventoryItem(potion, amount));

        InventoryEvents.OnInventoryChanged.Invoke();
    }

    public void RemovePotion(Potion potion, int amount)
    {
        if (potionInventory.ContainsKey(potion))
        {
            potionInventory[potion].quantity -= amount;
            if (potionInventory[potion].quantity <= 0)
                potionInventory.Remove(potion);
        }

        InventoryEvents.OnInventoryChanged.Invoke();
    }

    public List<InventoryItem> GetAllPotions()
    {
        return new List<InventoryItem>(potionInventory.Values);
    }

    public int GetPotionQuantity(Potion potion)
    {
        return potionInventory.ContainsKey(potion) ? potionInventory[potion].quantity : 0;
    }
}
