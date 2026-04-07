using UnityEngine;
using UnityEngine.InputSystem;

public class JournalUIManager : MonoBehaviour, PlayerControls.IUIActions
{
    [Header("UI References")]
    public GameObject journalVisuals; // The panel you want to show/hide

    private bool isOpen;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.UI.SetCallbacks(this);
    }

    void OnEnable()
    {
        controls.UI.Enable();
    }

    void OnDisable()
    {
        controls.UI.Disable();
    }

    void Start()
    {
        if (journalVisuals == null)
        {
            Debug.LogError("❌ Journal Visuals not assigned!");
            return;
        }

        journalVisuals.SetActive(false);
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        // Leave empty — handled by your Inventory UI
    }

    public void OnToggleJournal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleJournal();
        }
    }

    private void ToggleJournal()
    {
        isOpen = !isOpen;
        journalVisuals.SetActive(isOpen);

        Debug.Log($"📖 Journal toggled → {(isOpen ? "OPEN" : "CLOSED")}");
    }
}
