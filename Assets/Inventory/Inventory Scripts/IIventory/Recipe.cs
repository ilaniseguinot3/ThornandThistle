using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Crafting/Recipe")]
public class Recipe : ScriptableObject
{
    [System.Serializable]
    public struct IngredientRequirement
    {
        public Ingredient ingredient;
        public int requiredAmount;
    }

    public List<IngredientRequirement> ingredients;
    public Potion resultPotion;
    public int resultAmount = 1;
}
