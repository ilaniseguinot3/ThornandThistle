/*using System.Collections;
using UnityEngine;

public class CauldronClickHandler : MonoBehaviour
{
    public GameObject fire;

    private void OnMouseDown()
    {
        if (CauldronManager.Instance != null)
        {
            StartCoroutine(FireRoutine());
            CauldronManager.Instance.TryCombineIngredients();
            Debug.Log("🔮 Cauldron clicked — attempting brew!");
        }
    }

    private IEnumerator FireRoutine()
    {
        fire.SetActive(true);
        yield return new WaitForSeconds(2f);
        fire.SetActive(false);
    }
}
*/