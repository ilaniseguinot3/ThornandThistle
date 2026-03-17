using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CauldronManager : MonoBehaviour
{
    public static CauldronManager Instance { get; private set; }

    // --- OLD UI fields (remove from Inspector if you want, no longer used) ---
    // public Transform ingredientCircleParent;
    // public GameObject ingredientSlotPrefab;
    // public int circleRadius = 150;

    [Header("3D Ingredient Display")]
    public Transform cauldronCenter;        // Assign the cauldron's Transform in Inspector
    public Material ingredientQuadMaterial; // A Sprites/Default or Unlit/Transparent material
    public float orbitRadius = 0.6f;        // How far from cauldron center the quads orbit
    public float orbitHeight = 0.5f;        // How high above the cauldron they float
    public Vector3 quadScale = new Vector3(0.3f, 0.3f, 0.3f);

    [Header("Brewing")]
    public Button brewButton;
    public List<Recipe> allRecipes = new();

    private List<Ingredient> currentIngredients = new();
    private List<GameObject> spawnedQuads = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
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
        // Destroy old 3D quads
        foreach (var quad in spawnedQuads)
            if (quad != null) Destroy(quad);
        spawnedQuads.Clear();

        int count = currentIngredients.Count;
        if (count == 0 || cauldronCenter == null) return;

        for (int i = 0; i < count; i++)
        {
            // Calculate evenly spaced position around cauldron
            float angle = (360f / count) * i * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * orbitRadius;
            Vector3 worldPos = cauldronCenter.position + offset + Vector3.up * orbitHeight;

            // Spawn a primitive Quad
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = $"IngredientQuad_{currentIngredients[i].ingredientName}";
            quad.transform.position = worldPos;
            quad.transform.localScale = quadScale;

            // Assign material
            MeshRenderer mr = quad.GetComponent<MeshRenderer>();
            if (ingredientQuadMaterial != null)
                mr.material = new Material(ingredientQuadMaterial); // instance per quad
            else
                Debug.LogWarning("⚠️ No ingredientQuadMaterial assigned on CauldronManager!");

            // Attach 3D ingredient component
            CauldronIngredient3D ing3D = quad.AddComponent<CauldronIngredient3D>();
            ing3D.Initialize(currentIngredients[i]);

            spawnedQuads.Add(quad);
        }
    }

    public void RemoveIngredientAndReturn(Ingredient ingredient)
    {
        if (currentIngredients.Contains(ingredient))
        {
            currentIngredients.Remove(ingredient);
            UpdateIngredientCircleUI();
            InventoryManager.Instance.AddIngredient(ingredient, 1);
            Debug.Log($"🧺 Returned {ingredient.ingredientName} to inventory");
        }
        else
        {
            Debug.LogWarning("⚠ Ingredient not found in cauldron list!");
        }
    }

    private void TryCombineIngredients()
    {
        Debug.Log("🧪 Trying to combine ingredients...");

        if (currentIngredients.Count == 0)
        {
            Debug.Log("⚠️ No ingredients in the cauldron!");
            return;
        }

        foreach (var recipe in allRecipes)
        {
            bool matches = true;
            foreach (var req in recipe.ingredients)
            {
                if (!currentIngredients.Contains(req.ingredient))
                {
                    matches = false;
                    break;
                }
            }

            if (matches && recipe.ingredients.Count == currentIngredients.Count)
            {
                CraftingManager.Instance.Craft(recipe);
                ClearCauldron();
                Debug.Log($"✨ Brewed potion: {recipe.resultPotion.potionName}!");
                return;
            }
        }

        Debug.Log("❌ No valid recipe found for current ingredients.");
    }
}