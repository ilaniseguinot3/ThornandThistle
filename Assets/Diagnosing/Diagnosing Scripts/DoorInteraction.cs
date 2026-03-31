using UnityEngine;

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
                // Only allow click if no customer is active yet
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

    // Stage 1 — first click, arrival dialogue
    private void StartArrival()
    {
        CustomerManager.Instance.StartNextCustomer();
        currentCustomer = CustomerManager.Instance.CurrentCustomer;
        currentStage = Stage.ArrivalPlaying;

        DiagnosisUIManager.Instance.ShowIllness(currentCustomer.illness);

        DialogueManager.Instance.StartDialogue(currentCustomer.arrivalDialogue, onComplete: () =>
        {
            // Arrival dialogue done — player goes to brew
            currentStage = Stage.WaitingForPotion;
            Debug.Log("📜 Go brew a potion and return to the door!");
        });
    }

    // Stage 2 — second click, return dialogue before potion selection
    private void StartReturnDialogue()
    {
        currentStage = Stage.ReturnDialoguePlaying;

        DialogueManager.Instance.StartDialogue(currentCustomer.returnDialogue, onComplete: () =>
        {
            // Return dialogue done — now enable potion selection
            CustomerManager.Instance.EnterPotionSubmissionMode();
            Debug.Log("💊 Select a potion from the remedies panel!");

            CustomerManager.Instance.OnPotionEvaluated = (correct) =>
            {
                currentStage = Stage.ResponsePlaying;

                DialogueData response = correct
                    ? currentCustomer.correctPotionDialogue
                    : currentCustomer.wrongPotionDialogue;

                DialogueManager.Instance.StartDialogue(response, onComplete: () =>
                {
                    // All done — clean up and queue next customer
                    DiagnosisUIManager.Instance.HideIllness();
                    CustomerManager.Instance.FinishCurrentCustomer();
                    currentCustomer = null;
                    currentStage = Stage.Idle;
                    Debug.Log("✅ Customer done — next customer incoming!");
                });
            };
        });
    }
}