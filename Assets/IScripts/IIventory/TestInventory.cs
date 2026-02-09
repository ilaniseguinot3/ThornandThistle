using UnityEngine;

public class TestInventory : MonoBehaviour
{
    public Ingredient testIngredient1;
    public Ingredient testIngredient2;
    public Ingredient testIngredient3;

    void Start()
    {
        // Add some test items
        if (InventoryManager.Instance != null)
        {
            if (testIngredient1 != null)
                InventoryManager.Instance.AddItem(testIngredient1, 5);
            
            if (testIngredient2 != null)
                InventoryManager.Instance.AddItem(testIngredient2, 3);
            
            if (testIngredient3 != null)
                InventoryManager.Instance.AddItem(testIngredient3, 7);
                
            Debug.Log("🧪 Test ingredients added to inventory!");
        }
    }
}