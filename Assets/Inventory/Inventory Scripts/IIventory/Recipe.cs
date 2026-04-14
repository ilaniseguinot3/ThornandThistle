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
    public int goldReward = 15; // earned when this potion is given to a patient
}