using UnityEngine;

public class CauldronClickHandler : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (CauldronManager.Instance != null)
        {
            CauldronManager.Instance.TryCombineIngredients();
            Debug.Log("🔮 Cauldron clicked — attempting brew!");
        }
    }
}