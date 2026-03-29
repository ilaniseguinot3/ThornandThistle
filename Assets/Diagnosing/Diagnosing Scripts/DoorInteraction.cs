using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    private enum DoorState { WaitingForCustomer, CustomerArrived }
    private DoorState state = DoorState.WaitingForCustomer;

    private void OnMouseDown()
    {
        if (state == DoorState.WaitingForCustomer)
        {
            // First click — bring in customer and play arrival dialogue
            CustomerManager.Instance.StartNextCustomer();
            state = DoorState.CustomerArrived;
        }
        else if (state == DoorState.CustomerArrived)
        {
            // Second click — enter potion submission mode
            if (CustomerManager.Instance.WaitingForPotion) return;
            CustomerManager.Instance.EnterPotionSubmissionMode();
            state = DoorState.WaitingForCustomer; // reset for next customer
        }
    }
}