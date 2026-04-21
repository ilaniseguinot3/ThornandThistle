using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class tutorialManager : MonoBehaviour
{
    public clickableObject clicker;

    // canvases
    public GameObject movementCanvas;
    public GameObject firstPatientCanvas;
    public GameObject dialogueTutorialCanvas;
    public GameObject bookTutorialCanvas;
    public GameObject ingredientTutorialCanvas;
    public GameObject CauldronTutorial;
    public GameObject CauldronTutorial2;
    public GameObject remedyTutorial;
    public GameObject CompletedTutorial;
   
    public void showFirstPatientCanvas()
    {
        movementCanvas.SetActive(false);
        firstPatientCanvas.SetActive(true);
        clicker.tutorialNum = 1;
    }
    public void showDialogueTutorial()
    {
        firstPatientCanvas.SetActive(false);
        dialogueTutorialCanvas.SetActive(true);
    }
    public void showRecipeBookTutorial()
    {
        dialogueTutorialCanvas.SetActive(false);
        bookTutorialCanvas.SetActive(true);
    }
    public void showingredientTutorialCanvas()
    {
        bookTutorialCanvas.SetActive(false);
        ingredientTutorialCanvas.SetActive(true);
        clicker.tutorialNum = 2;
    }
    public void showCauldronTutorial()
    {
        ingredientTutorialCanvas.SetActive(false);
        CauldronTutorial.SetActive(true);
        clicker.tutorialNum = 3;
    }
    public void showCauldronTutorial2()
    {
        CauldronTutorial.SetActive(false);
        CauldronTutorial2.SetActive(true);
    }
    public void showremedyTutorial()
    {
        CauldronTutorial.SetActive(false);
        remedyTutorial.SetActive(true);
    }
    public void showCompletedTutorial()
    {
        remedyTutorial.SetActive(false);
        CompletedTutorial.SetActive(true);
    }
    public void CompleteTutorial()
    {
        CompletedTutorial.SetActive(false);
        clicker.tutorialNum = 4;
    }
}