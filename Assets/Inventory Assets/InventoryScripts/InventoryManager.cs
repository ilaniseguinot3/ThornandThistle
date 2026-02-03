using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory Settings")]
    public List<InventoryItem> equippedIngredients = new List<InventoryItem>();
    public int maxInventorySize = 20;

    [Header("UI References")]
    public GameObject inventoryPanel;
    public Transform inventoryContent;
    public GameObject inventoryItemPrefab;

    [Header("Hotkey Settings")]
#if !ENABLE_INPUT_SYSTEM
    public KeyCode inventoryKey = KeyCode.I;
#else
    public Key inventoryKey = Key.I;
    [Tooltip("Optional: Assign an Input Action for opening inventory")]
    public InputActionReference inventoryAction;
#endif

    private bool isInventoryOpen = false;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        if (inventoryAction != null)
        {
            inventoryAction.action.Enable();
            inventoryAction.action.performed += OnInventoryActionPerformed;
        }
#endif
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        if (inventoryAction != null)
        {
            inventoryAction.action.performed -= OnInventoryActionPerformed;
            inventoryAction.action.Disable();
        }
#endif
    }

#if ENABLE_INPUT_SYSTEM
    private void OnInventoryActionPerformed(InputAction.CallbackContext context)
    {
        ToggleInventory();
    }
#endif

    private void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    private void Update()
    {
        // Check for hotkey press
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current[inventoryKey].wasPressedThisFrame)
        {
            ToggleInventory();
        }
#else
        if (Input.GetKeyDown(inventoryKey))
        {
            ToggleInventory();
        }
#endif
    }

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(isInventoryOpen);
        }

        if (isInventoryOpen)
        {
            RefreshInventoryUI();
        }
    }

    public void OpenInventory()
    {
        isInventoryOpen = true;
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(true);
            RefreshInventoryUI();
        }
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false);
        }
    }

    public bool AddItem(InventoryItem item)
    {
        if (equippedIngredients.Count >= maxInventorySize)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        equippedIngredients.Add(item);
        Debug.Log($"Added {item.itemName} to inventory. Total items: {equippedIngredients.Count}");

        if (isInventoryOpen)
        {
            RefreshInventoryUI();
        }

        return true;
    }

    public void RemoveItem(InventoryItem item)
    {
        equippedIngredients.Remove(item);
        Debug.Log($"Removed {item.itemName} from inventory. Total items: {equippedIngredients.Count}");

        if (isInventoryOpen)
        {
            RefreshInventoryUI();
        }
    }

    public void RemoveItemAt(int index)
    {
        if (index >= 0 && index < equippedIngredients.Count)
        {
            InventoryItem item = equippedIngredients[index];
            equippedIngredients.RemoveAt(index);
            Debug.Log($"Removed {item.itemName} from inventory.");

            if (isInventoryOpen)
            {
                RefreshInventoryUI();
            }
        }
    }

    private void RefreshInventoryUI()
    {
        if (inventoryContent == null || inventoryItemPrefab == null)
        {
            return;
        }

        // Clear existing UI items
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        // Create UI item for each equipped ingredient
        for (int i = 0; i < equippedIngredients.Count; i++)
        {
            GameObject itemObj = Instantiate(inventoryItemPrefab, inventoryContent);
            InventoryItemUI itemUI = itemObj.GetComponent<InventoryItemUI>();

            if (itemUI != null)
            {
                itemUI.SetupItem(equippedIngredients[i], i);
            }
        }
    }

    public int GetInventoryCount()
    {
        return equippedIngredients.Count;
    }

    public bool HasItem(string itemName)
    {
        return equippedIngredients.Exists(item => item.itemName == itemName);
    }
}

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public IngredientType ingredientType;
    public int quantity = 1;

    public InventoryItem(string name, string description, Sprite icon, IngredientType type)
    {
        itemName = name;
        itemDescription = description;
        itemIcon = icon;
        ingredientType = type;
    }
}

public enum IngredientType
{
    Fire,
    Water,
    Earth,
    Wind,
    Lightning,
    Nature,
    Ice,
    Dark,
    Light,
    Poison
}