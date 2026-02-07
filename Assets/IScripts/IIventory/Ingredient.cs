using UnityEngine;

[CreateAssetMenu(fileName = "New Ingredient", menuName = "Inventory/Ingredient")]
public class Ingredient : ScriptableObject
{
    public string ingredientName;
    public Sprite icon;
    public string category;
    public int id; // unique identifier
}
