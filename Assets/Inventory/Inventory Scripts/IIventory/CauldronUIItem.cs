using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CauldronIngredient3D : MonoBehaviour
{
    private Ingredient ingredient;
    private Camera mainCamera;

    public void Initialize(Ingredient ing)
    {
        ingredient = ing;
        mainCamera = Camera.main;

        // Apply the ingredient icon as a texture to the quad
        if (ing.icon != null)
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            mr.GetPropertyBlock(block);
            block.SetTexture("_MainTex", ing.icon.texture);
            mr.SetPropertyBlock(block);
        }
        else
        {
            Debug.LogWarning($"⚠️ Ingredient '{ing.ingredientName}' has no icon assigned!");
        }
    }

    private void Update()
    {
        // Billboard: always face the camera
        if (mainCamera != null)
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
    }

    private void OnMouseDown()
    {
        if (ingredient == null) return;

        Debug.Log($"🔁 3D click — returning {ingredient.ingredientName} to inventory");
        CauldronManager.Instance.RemoveIngredientAndReturn(ingredient);
    }
}