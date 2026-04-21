using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


public class clickableObject : MonoBehaviour
{
    public float reach = 6f;
    public int tutorialNum;
    public GameObject diagnosisCanvas;
    public DialogueData dialogueToPlay;
    public playerMovementScript playerMovementMouse;
    public tutorialManager tutorial;
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

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
           tutorialNum = 0;
        }
        else
        {
           tutorialNum = 10;
        }
    }

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
                // for tutorial, check tutorialNum
                if (tutorialNum > 0)
                {
                    if (hit.collider.gameObject.CompareTag("door"))
                    {
                        // if in tutorial, play tutorial dialogue
                        if (tutorialNum == 1 || tutorialNum == 5)
                        {
                            // mouse stuff 
                            playerMovementMouse.activeMouse = false;
                            crosshairs.SetActive(false);
                            Cursor.lockState = CursorLockMode.None;

                            // tutorial stuff
                            if (tutorialNum == 1)
                            {
                                tutorialNum = 2;
                                tutorial.firstPatientCanvas.SetActive(false);
                                tutorial.bookTutorialCanvas.SetActive(true);
                            }
                            else
                            {   
                                tutorial.remedyTutorial.SetActive(false);
                            }
                        }
                    }

                    if (tutorialNum > 1)
                    {
                        if (hit.collider.gameObject.CompareTag("milk thistle"))
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

                        if (tutorialNum > 2)
                        {
                            if (hit.collider.gameObject.CompareTag("cauldron"))
                            {
                                StartCoroutine(PlayFire());
                                CauldronManager.Instance.TryCombineIngredients();
                                Debug.Log("Cauldron clicked — attempting brew!");
                            }
                        }
                    }
                }
                
            }
        }
    }

    IEnumerator PlayFire()
    {
        fire.SetActive(true);

        ParticleSystem ps = fire.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Play();

        yield return new WaitForSeconds(2f);

        fire.SetActive(false);
    }
}