using UnityEngine;

public class TestInventory : MonoBehaviour
{
    public Ingredient testIngredient1;
    public Ingredient testIngredient2;
    public Ingredient testIngredient3;
    public Ingredient testIngredient4;
    public Ingredient testIngredient5;
    public Ingredient testIngredient6;
    public Ingredient testIngredient7;
    public Ingredient testIngredient8;
    public Ingredient testIngredient9;

    void Start()
    {
        // Add some test items to the ingredient inventory
        if (InventoryManager.Instance != null)
        {
            if (testIngredient1 != null)
                InventoryManager.Instance.AddIngredient(testIngredient1, 1);

            if (testIngredient2 != null)
                InventoryManager.Instance.AddIngredient(testIngredient2, 1);

            if (testIngredient3 != null)
                InventoryManager.Instance.AddIngredient(testIngredient3, 1);
            if (testIngredient4 != null)
                InventoryManager.Instance.AddIngredient(testIngredient4, 1);
            if (testIngredient5 != null)
                InventoryManager.Instance.AddIngredient(testIngredient5, 1);
            if (testIngredient6 != null)
                InventoryManager.Instance.AddIngredient(testIngredient6, 1);
            if (testIngredient7 != null)
                InventoryManager.Instance.AddIngredient(testIngredient7, 1);
            if (testIngredient8 != null)
                InventoryManager.Instance.AddIngredient(testIngredient8, 1);
            if (testIngredient9 != null)
                InventoryManager.Instance.AddIngredient(testIngredient9, 1);


            Debug.Log("🧪 Test ingredients added to inventory!");
        }
        else
        {
            Debug.LogError("❌ InventoryManager instance not found in scene!");
        }
    }
}
