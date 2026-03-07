using UnityEngine;
using UnityEngine.InputSystem;

public class clickableObject : MonoBehaviour
{

    // how far away items can be clicked from
    public float reach = 3f;
    // canvases
    public GameObject diagnosisCanvas;
    public DialogueData dialogueToPlay;


    void Update()
    {
        // if mouse is not available for some reason return
        if (Mouse.current == null) return;

        // clicking objects
        // if player presses the lmb:
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // use raycasting to find the object
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            // if it is within reach
            if (Physics.Raycast(ray, out hit, reach))
            {
                // and it has clickable tag
                if (hit.collider.gameObject.CompareTag("door"))
                {
                    // open the diagnosis canvas
                    print("clicked on door!");
                    diagnosisCanvas.SetActive(true);
                    DialogueManager.Instance.StartDialogue(dialogueToPlay);
                }
                else if (hit.collider.gameObject.CompareTag("cauldron"))
                {
                    // do something
                    print("clicked on cauldron!");
                }
                else if (hit.collider.gameObject.CompareTag("shelf"))
                {
                    // do something
                    print("clicked on shelf!");
                }
            }
        }
    }
}