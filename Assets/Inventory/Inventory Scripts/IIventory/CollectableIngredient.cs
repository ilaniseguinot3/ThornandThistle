using UnityEngine;

/// <summary>
/// Attach to any world object you want the player to collect.
/// Assign the Ingredient SO in the Inspector, then set the quantity.
/// Click/interact with the object to add it to the inventory.
/// </summary>
public class CollectableIngredient : MonoBehaviour
{
    [Header("Ingredient Settings")]
    public Ingredient ingredient;

    [Range(1, 99)]
    public int quantity = 1;

    [Header("Collection Settings")]
    [Tooltip("Destroy the object after collecting?")]
    public bool destroyOnCollect = true;

    void OnMouseDown()
    {
        Collect();
    }

    public void Collect()
    {
        if (ingredient == null)
        {
            Debug.LogWarning($"{gameObject.name}: No Ingredient assigned!");
            return;
        }

        InventoryManager.Instance.AddIngredient(ingredient, quantity);

        if (destroyOnCollect)
            Destroy(gameObject);
    }
}