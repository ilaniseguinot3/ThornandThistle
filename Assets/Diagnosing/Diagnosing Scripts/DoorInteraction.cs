using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    private enum Stage { Idle, ArrivalPlaying, WaitingForPotion, ReturnDialoguePlaying, ResponsePlaying }
    private Stage currentStage = Stage.Idle;
    private Customer currentCustomer;
    public clickableObject clicker;
    public GameObject sorryButton;
    public DialogueUI dialogueUI;
    public playerMovementScript playerMovementMouse;
    public GameObject crosshairs;
    public tutorialManager tutorial;
    public GameObject gameOverCanvas;

    private void OnMouseDown()
    {
        // if in tutorial, play tutorial dialogue
        // if it's the tutorial, only work when it has the correct timing
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            if (clicker.tutorialNum == 1 || clicker.tutorialNum == 5)
            {
                // mouse stuff 
                //playerMovementMouse.activeMouse = false;
                //crosshairs.SetActive(false);
                //Cursor.lockState = CursorLockMode.None;

                // tutorial stuff
                if (clicker.tutorialNum == 1)
                {
                    clicker.tutorialNum = 2;
                    tutorial.firstPatientCanvas.SetActive(false);
                    tutorial.bookTutorialCanvas.SetActive(true);
                }
                else
                {   
                    tutorial.remedyTutorial.SetActive(false);
                }

                switch (currentStage)
                {
                    case Stage.Idle:
                        if (!CustomerManager.Instance.CustomerActive)
                            StartArrival();
                        else
                            Debug.Log("⏳ Wait for the next customer...");
                        break;

                    case Stage.WaitingForPotion:
                        StartReturnDialogue();
                        break;

                    case Stage.ArrivalPlaying:
                    case Stage.ReturnDialoguePlaying:
                    case Stage.ResponsePlaying:
                        Debug.Log("⏳ Wait for dialogue to finish");
                        break;
                }
            }
            else
                return;
        }
        else
        {
            //if (InventoryUIManager.IsOpen) return;
            //if (JournalUIManager.IsOpen) return;
            Debug.Log("unlocked!");
            //playerMovementMouse.activeMouse = false;
            //crosshairs.SetActive(false);
            //Cursor.lockState = CursorLockMode.None;
            //Cursor.visible = true;

            switch (currentStage)
            {
                case Stage.Idle:
                    if (!CustomerManager.Instance.CustomerActive)
                        StartArrival();
                    else
                        Debug.Log("⏳ Wait for the next customer...");
                    break;

                case Stage.WaitingForPotion:
                    StartReturnDialogue();
                    break;

                case Stage.ArrivalPlaying:
                case Stage.ReturnDialoguePlaying:
                case Stage.ResponsePlaying:
                    Debug.Log("⏳ Wait for dialogue to finish");
                    break;
            }
        }
        
    }

    private void StartArrival()
    {
        // Hide exclamation when player clicks door
        if (CustomerManager.Instance.exclamationObject != null){
            CustomerManager.Instance.exclamationObject.SetActive(false);
        }

        CustomerManager.Instance.StartNextCustomer();
        currentCustomer = CustomerManager.Instance.CurrentCustomer;
        currentStage = Stage.ArrivalPlaying;

        DiagnosisUIManager.Instance.ShowIllness(currentCustomer.illness);

        DialogueManager.Instance.StartDialogue(currentCustomer.arrivalDialogue, onComplete: () =>
        {
            currentStage = Stage.WaitingForPotion;
            Debug.Log("📜 Go brew a potion and return to the door!");
        });
    }

    private void StartReturnDialogue()
    {
        currentStage = Stage.ReturnDialoguePlaying;

        Debug.Log($"currentCustomer: {currentCustomer}");
        if (currentCustomer != null)
            Debug.Log($"returnDialogue: {currentCustomer.returnDialogue}");

        DialogueManager.Instance.StartDialogue(
            currentCustomer.returnDialogue,
            onComplete: null,
            onFrozen: () =>
            {
                CustomerManager.Instance.EnterPotionSubmissionMode();
                Debug.Log("💊 Select a potion — dialogue stays open!");

                CustomerManager.Instance.OnPotionEvaluated = (correct) =>
                {
                    currentStage = Stage.ResponsePlaying;

                    DialogueData response = correct
                        ? currentCustomer.correctPotionDialogue
                        : currentCustomer.wrongPotionDialogue;

                    DialogueManager.Instance.SwapDialogue(response, onComplete: () =>
                    {
                        DiagnosisUIManager.Instance.HideIllness();
                        CustomerManager.Instance.FinishCurrentCustomer();
                        currentCustomer = null;
                        currentStage = Stage.Idle;
                        Debug.Log("✅ Customer done — next customer incoming!");
                    });
                    if (SceneManager.GetActiveScene().name == "Tutorial")
                    {
                        // show end screen for tutorial
                        sorryButton.SetActive(false);
                        gameOverCanvas.SetActive(true);
                    }
                    
                };
               // show sorry button to close dialogue
               sorryButton.SetActive(true);
            }
        );
    }
    // need to also reset it back to play the same thing over and over again
    public void hideDiagnosis()
    {
        currentStage = Stage.WaitingForPotion;
        DialogueManager.Instance.EndDialogue();
        Debug.Log("hid dialogue");
        /*DialogueManager.Instance.CancelDialogue();
        currentStage = Stage.WaitingForPotion;
        playerMovementMouse.activeMouse = true;
        crosshairs.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        sorryButton.SetActive(false);
        DiagnosisUIManager.Instance.HideIllness();
        Debug.Log("hid dialogue and reset everything.");
        */
    }
}