using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform contentParent;
    public GameObject inventorySlotPrefab;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);
        RefreshInventoryUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen) RefreshInventoryUI();
    }

    public void RefreshInventoryUI()
    {
        // Clear old UI
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // Rebuild from InventoryManager
        var items = InventoryManager.Instance.GetAllItems();
        foreach (var item in items)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, contentParent);
            slot.transform.Find("Name").GetComponent<Text>().text = item.ingredient.ingredientName;
            slot.transform.Find("Quantity").GetComponent<Text>().text = "x" + item.quantity;
            slot.transform.Find("Icon").GetComponent<Image>().sprite = item.ingredient.icon;

            Button btn = slot.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                CauldronManager.Instance.AddIngredient(item.ingredient);
                InventoryManager.Instance.RemoveItem(item.ingredient, 1);
                RefreshInventoryUI();
            });
        }
    }
}
