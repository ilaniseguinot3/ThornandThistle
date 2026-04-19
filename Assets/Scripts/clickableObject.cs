using UnityEngine;
using UnityEngine.InputSystem;

public class clickableObject : MonoBehaviour
{
    public float reach = 6f;
    public GameObject diagnosisCanvas;
    public DialogueData dialogueToPlay;
    public playerMovementScript playerMovementMouse;
    public GameObject crosshairs;
    public GameObject fire;

    [Header("Ingredients")]
    public Ingredient MilkThistle;
    public Ingredient ComfreyLeaf;
    public Ingredient Calendula;
    public Ingredient Plantain;
    public Ingredient WhiteOakBark;
    public Ingredient Echinacea;
    public Ingredient Salve;
    public Ingredient Poultice;
    public Ingredient Tincture;

    void Update()
    {
        if (Mouse.current == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            int layerMask = ~LayerMask.GetMask("ClickableShelf");

            if (Physics.Raycast(ray, out hit, reach, layerMask))
            {
                if (hit.collider.gameObject.CompareTag("cauldron"))
                {
                    // Play the fire particle system
                    fire.SetActive(true);
                    ParticleSystem ps = fire.GetComponent<ParticleSystem>();
                    if(ps != null)
                        ps.Play();
                    CauldronManager.Instance.TryCombineIngredients();
                    Debug.Log("Cauldron clicked — attempting brew!");
                }
                if (hit.collider.gameObject.CompareTag("door"))
                {
                    print("clicked on door!");
                    playerMovementMouse.activeMouse = false;
                    diagnosisCanvas.SetActive(true);
                    crosshairs.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    DialogueManager.Instance.StartDialogue(dialogueToPlay);
                }
                else if (hit.collider.gameObject.CompareTag("milk thistle"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(MilkThistle, 1);
                }
                else if (hit.collider.gameObject.CompareTag("comfrey leaf"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(ComfreyLeaf, 1);
                }
                else if (hit.collider.gameObject.CompareTag("calendula"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(Calendula, 1);
                }
                else if (hit.collider.gameObject.CompareTag("plantain"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(Plantain, 1);
                }
                else if (hit.collider.gameObject.CompareTag("white oak bark"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(WhiteOakBark, 1);
                }
                else if (hit.collider.gameObject.CompareTag("echinacea"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(Echinacea, 1);
                }
                else if (hit.collider.gameObject.CompareTag("salve"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(Salve, 1);
                }
                else if (hit.collider.gameObject.CompareTag("poultice"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(Poultice, 1);
                }
                else if (hit.collider.gameObject.CompareTag("tincture"))
                {
                    hit.collider.gameObject.SetActive(false);
                    InventoryManager.Instance.AddIngredient(Tincture, 1);
                }
            }
        }
    }
}