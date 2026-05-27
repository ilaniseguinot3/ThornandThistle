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
    public int correctPotionReward = 0;
    public int wrongPotionPenalty = 1;

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
        if (exclamationObject != null)
            exclamationObject.SetActive(true);

        if (doorKnockSound != null)
            doorKnockSound.Play();
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

    // customer randomizers:
    public string[] customerNames;
    public Sprite[] skinSprites;
    public Sprite[] shirtSprites;
    public Sprite[] eyeSprites;
    public Sprite[] noseSprites;
    public Sprite[] mouthSprites;
    public Sprite[] hairSprites;
    public Sprite[] accessorySprites;
    public Illness[] illnesses;

    public Illness GetRandomIllness()
    {
        int index = UnityEngine.Random.Range(0, illnesses.Length);
        return illnesses[index];
    }

    /*
    public var GetRandomDialogue()
    {
        int dialogueIndex = UnityEngine.Random.Range(0, currentCustomer.illness.dialogueSets.Count);
        return currentCustomer.illness.dialogueSets[dialogueIndex];
    }
    */

    public string GetRandomCustomerName()
    {
        int index = UnityEngine.Random.Range(0, customerNames.Length);
        return customerNames[index];
    }

    public Sprite GetRandomSkinSprite()
    {
        int index = UnityEngine.Random.Range(0, skinSprites.Length);
        return skinSprites[index];
    }

    public Sprite GetRandomShirtSprite()
    {
        int index = UnityEngine.Random.Range(0, shirtSprites.Length);
        return shirtSprites[index];
    }

    public Sprite GetRandomEyesSprite()
    {
        int index = UnityEngine.Random.Range(0, eyeSprites.Length);
        return eyeSprites[index];
    }

    public Sprite GetRandomNoseSprite()
    {
        int index = UnityEngine.Random.Range(0, noseSprites.Length);
        return noseSprites[index];
    }

    public Sprite GetRandomMouthSprite()
    {
        int index = UnityEngine.Random.Range(0, mouthSprites.Length);
        return mouthSprites[index];
    }

    public Sprite GetRandomHairSprite()
    {
        int index = UnityEngine.Random.Range(0, hairSprites.Length);
        return hairSprites[index];
    }

    public Sprite GetRandomAccessorySprite()
    {
        int index = UnityEngine.Random.Range(0, accessorySprites.Length);
        return accessorySprites[index];
    }

    // blank customer template for randomization:
    public Customer customerTemplate;

    public void StartNextCustomer()
    {
        // NOTE need to add variable for total number of customers i think!
        if (remainingCustomers.Count == 0)
            ShufflePool();
        /*
        currentCustomer = remainingCustomers[0];
        remainingCustomers.RemoveAt(0);
        */
        // make new customer based on template and randomization
        currentCustomer = ScriptableObject.CreateInstance<Customer>();
        // randomize parts
        currentCustomer.customerName = GetRandomCustomerName();
        currentCustomer.skin = GetRandomSkinSprite();
        currentCustomer.shirt = GetRandomShirtSprite();
        currentCustomer.eyes = GetRandomEyesSprite();
        currentCustomer.nose = GetRandomNoseSprite();
        currentCustomer.mouth = GetRandomMouthSprite();
        currentCustomer.hair = GetRandomHairSprite();
        currentCustomer.accessory = GetRandomAccessorySprite();
        currentCustomer.illness = GetRandomIllness();

        // set random dialogue
        
        //var chosenSet = GetRandomDialogue();
        int dialogueIndex = UnityEngine.Random.Range(0, currentCustomer.illness.dialogueSets.Count);
        var chosenSet = currentCustomer.illness.dialogueSets[dialogueIndex];
        currentCustomer.arrivalDialogue = chosenSet.arrivalDialogue;
        currentCustomer.returnDialogue = chosenSet.returnDialogue;
        currentCustomer.correctPotionDialogue = chosenSet.correctPotionDialogue;
        currentCustomer.wrongPotionDialogue = chosenSet.wrongPotionDialogue;
        

        waitingForPotion = false;
        customerActive = true;
        GameState.Diagnosing = false;

        Debug.Log($"👤 Customer arrived: {currentCustomer.customerName}");
    }

    public void EnterPotionSubmissionMode()
    {
        if (currentCustomer == null) return;
        waitingForPotion = true;
        GameState.Diagnosing = true;
        Debug.Log("💊 Waiting for potion selection...");
    }

    public void EvaluatePotion(Potion selectedPotion)
    {
        if (!waitingForPotion || currentCustomer == null) return;

        waitingForPotion = false;
        GameState.Diagnosing = false;

        bool correct = selectedPotion == currentCustomer.illness.cure;
        InventoryManager.Instance.RemovePotion(selectedPotion, 1);

        if (correct)
        {
            if (correctPotionReward > 0)
                MoneyManager.Instance.Earn(correctPotionReward);
            Debug.Log($"✅ Correct! +{correctPotionReward}g");
        }
        else
        {
            MoneyManager.Instance.TrySpend(wrongPotionPenalty);
            Debug.Log($"❌ Wrong! -{wrongPotionPenalty}g");
        }

        OnPotionEvaluated?.Invoke(correct);
        OnPotionEvaluated = null;
    }

    public void ApplyPenalty()
    {
        MoneyManager.Instance.TrySpend(wrongPotionPenalty);
        Debug.Log($"💸 Penalty: -{wrongPotionPenalty}g");
    }

    public void FinishCurrentCustomer()
    {
        currentCustomer = null;
        customerActive = false;
        waitingForPotion = false;
        GameState.Diagnosing = false;

        if (remainingCustomers.Count == 0)
        {
            Debug.Log("🌙 All customers served — Day Over!");
            GameOverManager.Instance.ShowDayOver();
            return;
        }

        StartCoroutine(QueueNextCustomerAfterDelay());
    }

    [Header("Next Customer Cue")]
    public GameObject exclamationObject;
    public AudioSource doorKnockSound;

    private IEnumerator QueueNextCustomerAfterDelay()
    {
        float delay = UnityEngine.Random.Range(minDelay, maxDelay);
        Debug.Log($"⏳ Next customer in {delay:F1} seconds...");
        yield return new WaitForSeconds(delay);

        Debug.Log("🚪 A new customer is ready — click the door!");

        if (exclamationObject != null)
            exclamationObject.SetActive(true);

        if (doorKnockSound != null)
            doorKnockSound.Play();
    }
}