using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class Potion : ScriptableObject
{
    public string potionName;
    public Sprite icon;
    public string description;
}
