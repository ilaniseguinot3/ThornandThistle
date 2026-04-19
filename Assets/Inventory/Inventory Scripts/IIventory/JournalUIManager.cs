using UnityEngine;
using UnityEngine.InputSystem;

public class JournalUIManager : MonoBehaviour, PlayerControls.IUIActions
{
    [Header("UI References")]
    public GameObject journalVisuals;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isOpen;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.UI.SetCallbacks(this);
    }

    void OnEnable()  { controls.UI.Enable(); }
    void OnDisable() { controls.UI.Disable(); }

    void Start()
    {
        if (journalVisuals == null)
        {
            Debug.LogError("❌ Journal Visuals not assigned!");
            return;
        }
        journalVisuals.SetActive(false);
    }

    public void OnToggleInventory(InputAction.CallbackContext context) { }

    public void OnToggleJournal(InputAction.CallbackContext context)
    {
        if (context.performed) ToggleJournal();
    }

    private void ToggleJournal()
    {
        isOpen = !isOpen;
        journalVisuals.SetActive(isOpen);

        // Play open or close sound
        if (audioSource != null)
        {
            AudioClip clip = isOpen ? openSound : closeSound;
            if (clip != null) audioSource.PlayOneShot(clip);
        }

        Debug.Log($"📖 Journal toggled → {(isOpen ? "OPEN" : "CLOSED")}");
    }
}