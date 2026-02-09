using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIManager : MonoBehaviour, PlayerControls.IUIActions
{
    [Header("UI References")]
    [Tooltip("The visual container to show/hide (NOT the parent GameObject)")]
    public GameObject inventoryVisuals;  // 👈 Changed from inventoryPanel
    public Transform contentParent;
    public GameObject inventorySlotPrefab;

    private bool isOpen;
    private PlayerControls controls;

    void Awake()
    {
        controls = new PlayerControls();
        controls.UI.SetCallbacks(this);
        Debug.Log("✅ Input system initialized");
    }

    void OnEnable()
    {
        controls.UI.Enable();
        Debug.Log($"🔧 UI Map enabled: {controls.UI.enabled}");
    }
    
    void OnDisable()
    {
        controls.UI.Disable();
        Debug.Log("🔧 UI Map disabled");
    }

    void Start()
    {
        if (inventoryVisuals == null)
        {
            Debug.LogError("❌ Inventory Visuals not assigned!");
            return;
        }
        
        inventoryVisuals.SetActive(false);  // 👈 Toggle child, not parent
        RefreshInventoryUI();
        Debug.Log("🚀 InventoryUIManager started");
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        Debug.Log($"🎯 OnToggleInventory called! Phase: {context.phase}");
        
        if (context.performed)
        {
            Debug.Log("✅ Context.performed = true, toggling now!");
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryVisuals.SetActive(isOpen);
        Debug.Log($"🎒 Inventory toggled → {(isOpen ? "OPEN" : "CLOSED")}");
    }

    private void RefreshInventoryUI()
    {
        if (contentParent == null) return;
        
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    }
}