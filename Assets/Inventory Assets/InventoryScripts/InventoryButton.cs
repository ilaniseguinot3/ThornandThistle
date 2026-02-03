using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryButton : MonoBehaviour
{
    [Header("UI References")]
    public Button inventoryButton;
    public TextMeshProUGUI counterText;
    public GameObject counterBadge;

    private void Start()
    {
        if (inventoryButton != null)
        {
            inventoryButton.onClick.AddListener(OnButtonClicked);
        }

        UpdateCounter();
    }

    private void Update()
    {
        // Update the counter display
        UpdateCounter();
    }

    private void OnButtonClicked()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ToggleInventory();
        }
    }

    private void UpdateCounter()
    {
        if (InventoryManager.Instance == null)
            return;

        int count = InventoryManager.Instance.GetInventoryCount();

        // Update counter text
        if (counterText != null)
        {
            counterText.text = count.ToString();
        }

        // Show/hide badge based on count
        if (counterBadge != null)
        {
            counterBadge.SetActive(count > 0);
        }
    }

    private void OnDestroy()
    {
        if (inventoryButton != null)
        {
            inventoryButton.onClick.RemoveListener(OnButtonClicked);
        }
    }
}