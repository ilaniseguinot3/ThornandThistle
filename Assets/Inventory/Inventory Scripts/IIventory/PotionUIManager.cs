using System.Collections.Generic;
using UnityEngine;

public class PotionUIManager : MonoBehaviour
{
    public GameObject potionUIPrefab;  // prefab with PotionSlotUI
    public Transform contentParent;    // panel/grid parent

    private Dictionary<Potion, GameObject> potionUIObjects = new();

    private void OnEnable()
    {
        InventoryEvents.OnInventoryChanged.AddListener(UpdatePotionUI);
    }

    private void OnDisable()
    {
        InventoryEvents.OnInventoryChanged.RemoveListener(UpdatePotionUI);
    }

    public void UpdatePotionUI()
    {
        var potionItems = InventoryManager.Instance.GetAllPotions();

        // Remove UI objects that no longer exist
        foreach (var kvp in new Dictionary<Potion, GameObject>(potionUIObjects))
        {
            if (!potionItems.Exists(x => (Potion)x.item == kvp.Key))
            {
                Destroy(kvp.Value);
                potionUIObjects.Remove(kvp.Key);
            }
        }

        // Add or update UI objects
        foreach (var item in potionItems)
        {
            Potion potion = item.item as Potion;
            int quantity = item.quantity;

            if (!potionUIObjects.ContainsKey(potion))
            {
                GameObject uiObj = Instantiate(potionUIPrefab, contentParent);
                PotionSlotUI slotUI = uiObj.GetComponent<PotionSlotUI>();
                slotUI.Initialize(potion, quantity, () => OnPotionClicked(potion));
                potionUIObjects.Add(potion, uiObj);
            }
            else
            {
                potionUIObjects[potion].GetComponent<PotionSlotUI>().UpdateQuantity(quantity);
            }
        }
    }

    private void OnPotionClicked(Potion potion)
    {
        Debug.Log($"Clicked Potion: {potion.potionName}");
        // Implement drag-to-patient here, if GameState.Diagnosing is true
    }
}
