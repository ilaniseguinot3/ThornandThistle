using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    [Header("UI Screens")]
    public GameObject gameOverScreen;
    public GameObject dayOverScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (dayOverScreen != null) dayOverScreen.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverScreen != null) gameOverScreen.SetActive(true);
        Time.timeScale = 0f; // ← freeze the game
        Debug.Log("💀 Game Over screen shown");
    }

    public void ShowDayOver()
    {
        if (dayOverScreen != null) dayOverScreen.SetActive(true);
        Time.timeScale = 0f; // ← freeze the game
        Debug.Log("🌙 Day Over screen shown");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}