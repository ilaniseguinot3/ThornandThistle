using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    // Dictionary acts like a runtime database
    private Dictionary<Ingredient, InventoryItem> inventory = new Dictionary<Ingredient, InventoryItem>();

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

    // Add item to inventory
    public void AddItem(Ingredient ingredient, int amount)
    {
        if (inventory.ContainsKey(ingredient))
        {
            inventory[ingredient].quantity += amount;
        }
        else
        {
            inventory.Add(ingredient, new InventoryItem(ingredient, amount));
        }

        Debug.Log($"{ingredient.ingredientName} added: +{amount}");
    }

    // Remove item
    public void RemoveItem(Ingredient ingredient, int amount)
    {
        if (inventory.ContainsKey(ingredient))
        {
            inventory[ingredient].quantity -= amount;
            if (inventory[ingredient].quantity <= 0)
                inventory.Remove(ingredient);
        }
    }

    // Get item quantity
    public int GetQuantity(Ingredient ingredient)
    {
        return inventory.ContainsKey(ingredient) ? inventory[ingredient].quantity : 0;
    }

    // Access all items (e.g., for UI)
    public List<InventoryItem> GetAllItems()
    {
        return new List<InventoryItem>(inventory.Values);
    }
}
