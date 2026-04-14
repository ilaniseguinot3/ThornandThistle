using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class Potion : ScriptableObject
{
    public string potionName;
    public Sprite icon;
    public int quantity;
    public Recipe sourceRecipe; // assign in Inspector — used to look up goldReward
}