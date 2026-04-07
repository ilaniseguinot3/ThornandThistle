using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance { get; private set; }

    [Header("Customer Pool")]
    public List<Customer> customerPool = new();

    [Header("Money")]
    public int correctPotionReward = 10;
    public int wrongPotionPenalty = 5;

    [Header("Next Customer Delay")]
    public float minDelay = 2f;
    public float maxDelay = 5f;

    private List<Customer> remainingCustomers = new();
    private Customer currentCustomer;
    private bool waitingForPotion = false;
    private bool customerActive = false;

    public Customer CurrentCustomer => currentCustomer;
    public bool WaitingForPotion => waitingForPotion;
    public bool CustomerActive => customerActive;
    public Action<bool> OnPotionEvaluated;

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
            int j = UnityEngine.Random.Range(0, i + 1);
            (remainingCustomers[i], remainingCustomers[j]) = (remainingCustomers[j], remainingCustomers[i]);
        }
        Debug.Log("🔀 Customer pool shuffled");
    }

    // Called on first door click
    public void StartNextCustomer()
    {
        if (remainingCustomers.Count == 0)
            ShufflePool();

        currentCustomer = remainingCustomers[0];
        remainingCustomers.RemoveAt(0);
        waitingForPotion = false;
        customerActive = true;
        GameState.Diagnosing = false;

        Debug.Log($"👤 Customer arrived: {currentCustomer.customerName}");
    }

    // Called on second door click
    public void EnterPotionSubmissionMode()
    {
        if (currentCustomer == null) return;
        waitingForPotion = true;
        GameState.Diagnosing = true;
        Debug.Log("💊 Waiting for potion selection...");
    }

    // Called from PotionSlotUI when GameState.Diagnosing is true
    public void EvaluatePotion(Potion selectedPotion)
    {
        if (!waitingForPotion || currentCustomer == null) return;

        waitingForPotion = false;
        GameState.Diagnosing = false;

        bool correct = selectedPotion == currentCustomer.illness.cure;
        InventoryManager.Instance.RemovePotion(selectedPotion, 1);

        if (correct)
        {
            // Add your money hook here
            Debug.Log($"✅ Correct! +{correctPotionReward} gold");
            // e.g. MoneyManager.Instance.AddMoney(correctPotionReward);
        }
        else
        {
            Debug.Log($"❌ Wrong! -{wrongPotionPenalty} gold");
            // e.g. MoneyManager.Instance.DeductMoney(wrongPotionPenalty);
        }

        OnPotionEvaluated?.Invoke(correct);
        OnPotionEvaluated = null;
    }

    public void ApplyPenalty()
    {
        Debug.Log($"💸 Penalty: -{wrongPotionPenalty}");
        // e.g. MoneyManager.Instance.DeductMoney(wrongPotionPenalty);
    }

    // Called after response dialogue ends
    public void FinishCurrentCustomer()
    {
        currentCustomer = null;
        customerActive = false;
        waitingForPotion = false;
        GameState.Diagnosing = false;
        StartCoroutine(QueueNextCustomerAfterDelay());
    }

    private IEnumerator QueueNextCustomerAfterDelay()
    {
        float delay = UnityEngine.Random.Range(minDelay, maxDelay);
        Debug.Log($"⏳ Next customer in {delay:F1} seconds...");
        yield return new WaitForSeconds(delay);
        Debug.Log("🚪 A new customer is ready — click the door!");
        // You can trigger a visual/audio cue here e.g. door knock sound
    }
}