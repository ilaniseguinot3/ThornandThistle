using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [Header("Starting Reputation")]
    public int startingGold = 3;

    private int currentGold;
    public int CurrentGold => currentGold;

    public static readonly UnityEvent OnMoneyChanged = new UnityEvent();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        currentGold = startingGold;
    }

    /// <summary>Returns false if the player can't afford it.</summary>
    public bool TrySpend(int amount)
    {
        if (amount > currentGold)
        {
            Debug.Log($"❌ Not enough gold! Need {amount}, have {currentGold}");
            return false;
        }
        currentGold -= amount;
        OnMoneyChanged.Invoke();
        Debug.Log($"💰 Spent {amount}g — remaining: {currentGold}g");
        return true;
    }

    public void Earn(int amount)
    {
        currentGold += amount;
        OnMoneyChanged.Invoke();
        Debug.Log($"💰 Earned {amount}g — total: {currentGold}g");
    }
}