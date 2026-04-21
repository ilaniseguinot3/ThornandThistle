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

    private void OnMouseDown()
    {
        // if it's the tutorial, only work when it has the correct timing
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            Debug.Log("StartReturnDialogue called in tutorial.");
            if (clicker.tutorialNum == 1 || clicker.tutorialNum == 5)
            {
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
            if (InventoryUIManager.IsOpen) return;
            if (JournalUIManager.IsOpen) return;

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
                };
               // show sorry button to close dialogue
               sorryButton.SetActive(true);
            }
        );
    }
    // need to also reset it back to play the same thing over and over again
    public void hideDiagnosis()
    {
        dialogueUI.Hide();
        playerMovementMouse.activeMouse = true;
        crosshairs.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        sorryButton.SetActive(false);
        Debug.Log("hid dialogue");
    }
}