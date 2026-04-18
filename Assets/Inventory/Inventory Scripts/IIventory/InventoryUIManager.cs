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
        DontDestroyOnLoad(gameObject);
        Debug.Log("✅ Input system initialized");
    }

    void OnEnable()
    {
        controls.UI.Enable();
        InventoryEvents.OnInventoryChanged.AddListener(RefreshInventoryUI);
        Debug.Log("🔧 UI Map enabled and subscribed to inventory updates");
    }

    void OnDisable()
    {
        controls.UI.Disable();
        InventoryEvents.OnInventoryChanged.RemoveListener(RefreshInventoryUI);
        Debug.Log("🔧 UI Map disabled and unsubscribed from inventory updates");
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
        if (!isOpen) return;

        // DestroyImmediate ensures children are gone before we rebuild
        for (int i = contentParent.childCount - 1; i >= 0; i--)
            DestroyImmediate(contentParent.GetChild(i).gameObject);

        var allItems = InventoryManager.Instance.GetAllIngredients();

        foreach (var inventoryItem in allItems)
        {
            if (!(inventoryItem.item is Ingredient ingredient))
            {
                Debug.LogWarning("InventoryItem is not an Ingredient!");
                continue;
            }

            GameObject slotObj = Instantiate(inventorySlotPrefab, contentParent);
            InventorySlotUI slotUI = slotObj.GetComponent<InventorySlotUI>();
            slotUI.Initialize(ingredient, inventoryItem.quantity, OnIngredientClicked);
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
            InventoryManager.Instance.RemoveIngredient(ingredient, 1);
            
            // Refresh UI to show updated quantity
            RefreshInventoryUI();
        }
        else
        {
            Debug.LogWarning("⚠️ CauldronManager.Instance is null!");
        }
    }
    public void OnToggleJournal(InputAction.CallbackContext context)
    {
        // Do nothing — handled by JournalUIManager instead
    }

}