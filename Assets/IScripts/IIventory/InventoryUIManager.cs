using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIManager : MonoBehaviour, PlayerControls.IUIActions
{
    [Header("UI References")]
    [Tooltip("The visual container to show/hide (NOT the parent GameObject)")]
    public GameObject inventoryVisuals;
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
        
        inventoryVisuals.SetActive(false);
        RefreshInventoryUI();
        Debug.Log("🚀 InventoryUIManager started");
    }

    public void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("✅ Toggling inventory!");
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryVisuals.SetActive(isOpen);
        
        // Refresh UI every time we open it
        if (isOpen)
        {
            RefreshInventoryUI();
        }
        
        Debug.Log($"🎒 Inventory toggled → {(isOpen ? "OPEN" : "CLOSED")}");
    }

    private void RefreshInventoryUI()
    {
        if (contentParent == null)
        {
            Debug.LogError("❌ Content Parent not assigned!");
            return;
        }

        if (inventorySlotPrefab == null)
        {
            Debug.LogError("❌ Inventory Slot Prefab not assigned!");
            return;
        }

        // Clear existing slots
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Check if InventoryManager exists
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("⚠️ InventoryManager.Instance is null! No items to display.");
            return;
        }

        // Get all items from inventory
        var allItems = InventoryManager.Instance.GetAllItems();
        
        Debug.Log($"📦 Found {allItems.Count} items in inventory");

        // Create a slot for each item
        foreach (var item in allItems)
        {
            GameObject slotObj = Instantiate(inventorySlotPrefab, contentParent);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();

            if (slotUI != null)
            {
                // Initialize the slot with ingredient data
                slotUI.Initialize(item.ingredient, item.quantity, OnIngredientClicked);
                Debug.Log($"✅ Created slot for: {item.ingredient.ingredientName} x{item.quantity}");
            }
            else
            {
                Debug.LogError("❌ InventorySlotUI component not found on prefab!");
            }
        }
    }

    // Callback when an ingredient is clicked in the inventory
    private void OnIngredientClicked(Ingredient ingredient)
    {
        Debug.Log($"🖱️ Clicked on: {ingredient.ingredientName}");
        
        // Add to cauldron if CauldronManager exists
        if (CauldronManager.Instance != null)
        {
            CauldronManager.Instance.AddIngredient(ingredient);
            
            // Remove from inventory
            InventoryManager.Instance.RemoveItem(ingredient, 1);
            
            // Refresh UI to show updated quantity
            RefreshInventoryUI();
        }
        else
        {
            Debug.LogWarning("⚠️ CauldronManager.Instance is null!");
        }
    }
}