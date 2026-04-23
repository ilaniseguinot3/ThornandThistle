using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeBook : MonoBehaviour
{
    [Header("Recipes")]
    // Reference your ScriptableObject assets here!
    public List<Recipe> recipes = new List<Recipe>();

    [Header("UI References")]
    public Image recipeImageDisplay;
    public TextMeshProUGUI recipeTitleText;
    public TextMeshProUGUI descriptionText;
    // (ingredientImages/labels can be removed if not used)

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

        if (recipeImageDisplay != null)
        {
            recipeImageDisplay.sprite = r.recipeImage;
            recipeImageDisplay.enabled = r.recipeImage != null;
        }
    }
}