using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    private enum Stage { Idle, ArrivalPlaying, WaitingForPotion, ReturnDialoguePlaying, ResponsePlaying }
    private Stage currentStage = Stage.Idle;
    private Customer currentCustomer;

    private void OnMouseDown()
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
            }
        );
    }
}