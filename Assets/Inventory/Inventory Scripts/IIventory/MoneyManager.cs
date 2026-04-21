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

    public bool TrySpend(int amount)
    {
        currentGold -= amount;
        if (currentGold < 0) currentGold = 0;
        OnMoneyChanged.Invoke();
        Debug.Log($"💰 Spent {amount}g — remaining: {currentGold}g");

        if (currentGold <= 0)
        {
            Debug.Log("💀 Reputation hit 0 — Game Over!");
            GameOverManager.Instance.ShowGameOver();
        }

        return true;
    }

    public void Earn(int amount)
    {
        currentGold += amount;
        OnMoneyChanged.Invoke();
        Debug.Log($"💰 Earned {amount}g — total: {currentGold}g");
    }
}