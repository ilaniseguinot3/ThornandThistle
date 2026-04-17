using UnityEngine;
using UnityEngine.InputSystem;

public class clickableObject : MonoBehaviour
{

    // how far away items can be clicked from
    public float reach = 6f;
    // canvases
    public GameObject diagnosisCanvas;
    public DialogueData dialogueToPlay;

    public playerMovementScript playerMovementMouse;
    public GameObject crosshairs;
    public GameObject fire;


    void Update()
    {
        // if mouse is not available for some reason return
        if (Mouse.current == null) return;

        // clicking objects
        // if player presses the lmb:
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // use raycasting to find the object
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // add mask to ignore shelf
            int layerMask = ~LayerMask.GetMask("ClickableShelf");

           

            // if it is within reach
            if (Physics.Raycast(ray, out hit, reach, layerMask))
            {
                //Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                // and it has clickable tag
                if (hit.collider.gameObject.CompareTag("door"))
                {
                    // open the diagnosis canvas
                    print("clicked on door!");
                    playerMovementMouse.activeMouse = false;
                    diagnosisCanvas.SetActive(true);
                    crosshairs.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    DialogueManager.Instance.StartDialogue(dialogueToPlay);
                }
                //else if (hit.collider.gameObject.CompareTag("cauldron"))
                //{
                //    fire.SetActive(true);
                //}
                else if (hit.collider.gameObject.CompareTag("milk thistle"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("comfrey leaf"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("calendula"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("plantain"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("white oak bark"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("echinacea"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("salve"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("poultice"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
                else if (hit.collider.gameObject.CompareTag("tincture"))
                {
                    hit.collider.gameObject.SetActive(false);
                    // add to inventory
                }
            }
        }
    }
}