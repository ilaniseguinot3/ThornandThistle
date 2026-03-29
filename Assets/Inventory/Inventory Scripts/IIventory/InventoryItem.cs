using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ScriptableObject item; // can be Potion or Ingredient
    public int quantity;

    public InventoryItem(ScriptableObject newItem, int amount)
    {
        item = newItem;
        quantity = amount;
    }
}
