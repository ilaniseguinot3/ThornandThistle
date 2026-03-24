using UnityEngine;

public class TestInventory : MonoBehaviour
{
    public Ingredient testIngredient1;
    public Ingredient testIngredient2;
    public Ingredient testIngredient3;
    public Ingredient testIngredient4;

    void Start()
    {
        // Add some test items to the ingredient inventory
        if (InventoryManager.Instance != null)
        {
            if (testIngredient1 != null)
                InventoryManager.Instance.AddIngredient(testIngredient1, 5);

            if (testIngredient2 != null)
                InventoryManager.Instance.AddIngredient(testIngredient2, 3);

            if (testIngredient3 != null)
                InventoryManager.Instance.AddIngredient(testIngredient3, 7);
            if (testIngredient4 != null)
                InventoryManager.Instance.AddIngredient(testIngredient4, 4);

            Debug.Log("🧪 Test ingredients added to inventory!");
        }
        else
        {
            Debug.LogError("❌ InventoryManager instance not found in scene!");
        }
    }
}
