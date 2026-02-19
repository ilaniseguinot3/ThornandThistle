using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CauldronManager : MonoBehaviour
{
    public static CauldronManager Instance { get; private set; }

    public Transform ingredientCircleParent;
    public GameObject ingredientSlotPrefab;
    public int circleRadius = 150;
    public Button brewButton;                          // 👈 Add this
    public List<Recipe> allRecipes = new();            // 👈 Reference all recipes (assign in Inspector)

    private List<Ingredient> currentIngredients = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (brewButton != null)
            brewButton.onClick.AddListener(TryCombineIngredients);
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

            // Position the icon in a circle
            RectTransform rt = iconObj.GetComponent<RectTransform>();
            rt.anchoredPosition = pos;

            // Set the ingredient sprite
            var image = iconObj.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
                image.sprite = currentIngredients[i].icon;

            // Make the icon clickable
            var button = iconObj.GetComponent<UnityEngine.UI.Button>();
            if (button == null)
                button = iconObj.AddComponent<UnityEngine.UI.Button>();

            Ingredient ingredientRef = currentIngredients[i]; // capture loop variable

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                Debug.Log($"🔁 Returned {ingredientRef.ingredientName} to inventory");
                RemoveIngredientAndReturn(ingredientRef);
            });
        }
    }

    public void RemoveIngredientAndReturn(Ingredient ingredient)
    {
        if (currentIngredients.Contains(ingredient))
        {
            currentIngredients.Remove(ingredient);
            UpdateIngredientCircleUI();

            // Return to inventory
            InventoryManager.Instance.AddIngredient(ingredient, 1);

            Debug.Log($"🧺 Returned {ingredient.ingredientName} to inventory");
        }
        else
        {
            Debug.LogWarning("⚠ Ingredient not found in cauldron list!");
        }
    }

    

    // 🔮 New method to check combinations
    private void TryCombineIngredients()
    {
        Debug.Log("🧪 Trying to combine ingredients...");

        if (currentIngredients.Count == 0)
        {
            Debug.Log("⚠️ No ingredients in the cauldron!");
            return;
        }

        // Check every recipe in your list
        foreach (var recipe in allRecipes)
        {
            bool matches = true;

            foreach (var req in recipe.ingredients)
            {
                // Does this recipe require an ingredient not in the cauldron?
                if (!currentIngredients.Contains(req.ingredient))
                {
                    matches = false;
                    break;
                }
            }

            // Check quantity match (optional)
            if (matches && recipe.ingredients.Count == currentIngredients.Count)
            {
                CraftingManager.Instance.Craft(recipe);
                ClearCauldron();
                Debug.Log($"✨ Brewed potion: {recipe.resultPotion.potionName}!");
                // Show the potion result visually
                GameObject potionIcon = new GameObject("PotionIcon", typeof(RectTransform), typeof(Image));
                potionIcon.transform.SetParent(ingredientCircleParent);
                var img = potionIcon.GetComponent<Image>();
                img.sprite = recipe.resultPotion.icon;
                img.rectTransform.anchoredPosition = Vector2.zero;

                return;
            }
        }

        Debug.Log("❌ No valid recipe found for current ingredients.");
    }
}
