using UnityEngine;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }

    [Header("Customer Pool")]
    public List<Customer> customerPool = new();

    [Header("Penalty")]
    public int wrongPotionPenalty = 5; // gold deducted

    private List<Customer> remainingCustomers = new();
    private Customer currentCustomer;
    private bool waitingForPotion = false;

    public Customer CurrentCustomer => currentCustomer;
    public bool WaitingForPotion => waitingForPotion;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        ShufflePool();
    }

    private void ShufflePool()
    {
        remainingCustomers = new List<Customer>(customerPool);
        for (int i = remainingCustomers.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (remainingCustomers[i], remainingCustomers[j]) = (remainingCustomers[j], remainingCustomers[i]);
        }
    }

    // Called on first door click
    public void StartNextCustomer()
    {
        if (remainingCustomers.Count == 0)
            ShufflePool(); // reshuffle when all customers have been seen

        currentCustomer = remainingCustomers[0];
        remainingCustomers.RemoveAt(0);

        waitingForPotion = false;
        GameState.Diagnosing = false;

        // Show illness UI
        DiagnosisUIManager.Instance.ShowIllness(currentCustomer.illness);

        // Trigger arrival dialogue
        DialogueManager.Instance.StartDialogue(currentCustomer.arrivalDialogue);

        Debug.Log($"👤 New customer: {currentCustomer.customerName} | Illness: {currentCustomer.illness.illnessName}");
    }

    // Called on second door click
    public void EnterPotionSubmissionMode()
    {
        if (currentCustomer == null) return;
        waitingForPotion = true;
        GameState.Diagnosing = true;
        Debug.Log("💊 Waiting for potion selection...");
    }

    // Called when player clicks a potion during diagnosing
    public void EvaluatePotion(Potion selectedPotion)
    {
        if (!waitingForPotion || currentCustomer == null) return;

        waitingForPotion = false;
        GameState.Diagnosing = false;

        bool correct = selectedPotion == currentCustomer.illness.cure;

        if (correct)
        {
            Debug.Log($"✅ Correct potion given!");
            DialogueManager.Instance.StartDialogue(currentCustomer.correctPotionDialogue);
            InventoryManager.Instance.RemovePotion(selectedPotion, 1);
        }
        else
        {
            Debug.Log($"❌ Wrong potion! Applying penalty of {wrongPotionPenalty}.");
            DialogueManager.Instance.StartDialogue(currentCustomer.wrongPotionDialogue);
            InventoryManager.Instance.RemovePotion(selectedPotion, 1);
            ApplyPenalty();
        }

        DiagnosisUIManager.Instance.HideIllness();
        currentCustomer = null;
    }

    private void ApplyPenalty()
    {
        // Hook into your money/reputation system here
        Debug.Log($"💸 Penalty applied: -{wrongPotionPenalty}");
    }
}