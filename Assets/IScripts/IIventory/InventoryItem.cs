[System.Serializable]
public class InventoryItem
{
    public Ingredient ingredient;
    public int quantity;

    public InventoryItem(Ingredient ingredient, int quantity)
    {
        this.ingredient = ingredient;
        this.quantity = quantity;
    }
}
