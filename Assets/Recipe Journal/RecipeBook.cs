using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeBook : MonoBehaviour
{
    [System.Serializable]
    public class IngredientEntry
    {
        public string ingredientName;
        public Sprite ingredientSprite;
    }

    [System.Serializable]
    public class Recipe
    {
        public string recipeName;
        [TextArea(2, 5)]
        public string description;
        public List<IngredientEntry> ingredients = new List<IngredientEntry>();
    }

    [Header("Recipes")]
    public List<Recipe> recipes = new List<Recipe>();

    [Header("UI References")]
    public TextMeshProUGUI recipeTitleText;
    public TextMeshProUGUI descriptionText;
    public List<Image> ingredientImages = new List<Image>();
    public List<TextMeshProUGUI> ingredientLabels = new List<TextMeshProUGUI>();

    [Header("Navigation")]
    public Button nextButton;
    public Button prevButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pageTurnSound;

    private int _currentIndex = 0;

    void Start()
    {
        nextButton?.onClick.AddListener(OnNextClicked);
        prevButton?.onClick.AddListener(OnPrevClicked);
        DisplayRecipe(_currentIndex);
    }

    void OnNextClicked()
    {
        _currentIndex = (_currentIndex + 1) % recipes.Count;
        PlayPageTurn();
        DisplayRecipe(_currentIndex);
    }

    void OnPrevClicked()
    {
        _currentIndex = (_currentIndex - 1 + recipes.Count) % recipes.Count;
        PlayPageTurn();
        DisplayRecipe(_currentIndex);
    }

    void PlayPageTurn()
    {
        if (audioSource != null && pageTurnSound != null)
            audioSource.PlayOneShot(pageTurnSound);
    }

    void DisplayRecipe(int index)
    {
        Recipe r = recipes[index];

        if (recipeTitleText) recipeTitleText.text = r.recipeName;
        if (descriptionText)  descriptionText.text  = r.description;

        for (int i = 0; i < ingredientImages.Count; i++)
        {
            bool hasIngredient = i < r.ingredients.Count;

            if (ingredientImages[i])
            {
                ingredientImages[i].sprite  = hasIngredient ? r.ingredients[i].ingredientSprite : null;
                ingredientImages[i].enabled = hasIngredient;
            }

            if (i < ingredientLabels.Count && ingredientLabels[i])
            {
                ingredientLabels[i].text = hasIngredient ? r.ingredients[i].ingredientName : "";
            }
        }
    }
}