using UnityEngine;
using System.Collections.Generic;

public class CauldronManager : MonoBehaviour
{
    public static CauldronManager Instance { get; private set; }

    public Transform ingredientCircleParent;
    public GameObject ingredientSlotPrefab;
    public int circleRadius = 150;

    private List<Ingredient> currentIngredients = new List<Ingredient>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        currentIngredients.Add(ingredient);
        UpdateIngredientCircleUI();

        Debug.Log("Added to cauldron: " + ingredient.ingredientName);
    }

    public void ClearCauldron()
    {
        currentIngredients.Clear();
        UpdateIngredientCircleUI();
    }

    public void UpdateIngredientCircleUI()
    {
        // Clear old icons
        foreach (Transform child in ingredientCircleParent)
            Destroy(child.gameObject);

        int count = currentIngredients.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject iconObj = Instantiate(ingredientSlotPrefab, ingredientCircleParent);
            float angle = (360f / count) * i * Mathf.Deg2Rad;
            Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * circleRadius;
            iconObj.GetComponent<RectTransform>().anchoredPosition = pos;
            iconObj.GetComponent<UnityEngine.UI.Image>().sprite = currentIngredients[i].icon;
        }
    }

    public void TryCraftPotion(Recipe recipe)
    {
        // Compare cauldron ingredients with recipe
        if (CraftingManager.Instance.CanCraft(recipe))
        {
            CraftingManager.Instance.Craft(recipe);
            ClearCauldron();
        }
        else
        {
            Debug.Log("Not enough or wrong ingredients for " + recipe.resultPotion.potionName);
        }
    }
}
