using UnityEngine;
using UnityEngine.InputSystem;

public class dragAndDrop : MonoBehaviour
{

    // how far away items can be picked up from
    public float reach = 3f;
    // storage for where the object is being moved
    private Transform objectMoved = null;

    void Update()
    {
        // if mouse is not available for some reason return
        if (Mouse.current == null) return;

        // picking up objects
        // if player is not holding anything and presses the lmb:
        if (Mouse.current.leftButton.wasPressedThisFrame && objectMoved == null)
        {
            // use raycasting to find the object
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            // if it is within reach
            if (Physics.Raycast(ray, out hit, reach))
            {
                // and rigidbody exists
                if (hit.collider.GetComponent<Rigidbody>() != null)
                {
                    // store the object's transform and turn off its gravity so it can be moved
                    objectMoved = hit.transform;
                    hit.collider.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }

        // moving objects
        if (objectMoved != null)
        {
            // update the object's position based on the Camera
            objectMoved.position = Camera.main.transform.position + Camera.main.transform.forward * reach;

            // drop the object
            // check if the lmb was just released
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                // if it has a rigidbody set the gravity back on
                if(objectMoved.GetComponent<Rigidbody>())
                    objectMoved.GetComponent<Rigidbody>().useGravity = true;
                // clear objectMoved
                objectMoved = null;
            }
        }
    }
}