using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class playerMovementScript : MonoBehaviour
{
    public GameObject crosshairs;
    public bool activeMouse;
    public 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if tutorial scene do opposite
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            // leave the cursor unlocked
            Cursor.lockState = CursorLockMode.Locked;
            // hide crosshairs
            crosshairs.SetActive(true);
            activeMouse = true;
        }
        else
        {
            // leave the cursor unlocked
            Cursor.lockState = CursorLockMode.None;
            // hide crosshairs
            crosshairs.SetActive(false);
            activeMouse = false;
        }
    }

     
    // set default speeds
    float horizontalSpeed = 0.1f;
    float verticalSpeed = 0.1f;
    public int playerSpeed = 9;

    // save yaw and pitch (camera rotation)
    float yaw = 0f;
    float pitch = 0f;

    // Update is called once per frame
    void Update()
    {        
        if (activeMouse)
        {
            // get current key pressed
            var key = Keyboard.current;
            // if nothing return
            if (key == null)
                return;

            else if (key.leftShiftKey.isPressed)
            {
                crosshairs.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {   
                crosshairs.SetActive(true);
                // Lock the cursor to the center of the screen
                Cursor.lockState = CursorLockMode.Locked;

                // check if mouse has moved, if it has, rotate the camera and update yaw and pitch
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();

                yaw += mouseDelta.x * horizontalSpeed;
                pitch -= mouseDelta.y * verticalSpeed;

                // clamp pitch so camera cannot invert
                pitch = Mathf.Clamp(pitch, -89f, 89f);

                transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

                // create direction vectors based on local foward and right directions
                Vector3 facingDirection = this.transform.forward;
                Vector3 rightDirection = this.transform.right;

                // lock the Y axis
                facingDirection.y = 0f;
                rightDirection.y = 0f;

                // Normalize the direction vectors
                facingDirection.Normalize();
                rightDirection.Normalize();

                // make a new vector that will be used to move the character
                Vector3 move = Vector3.zero;

                // update this vector based on what key(s) are pressed
                if(key.wKey.isPressed)
                    move += facingDirection;
                if(key.sKey.isPressed)
                    move -= facingDirection;
                if(key.aKey.isPressed)
                    move -= rightDirection;
                if(key.dKey.isPressed)
                    move += rightDirection;


                // also check if it will cause a collision, only move if it doesn't
                // get the direction
                Vector3 moveDirection = move.normalized;
                // get the distance
                float moveDistance = move.magnitude * playerSpeed * Time.deltaTime;

                // use raycasting to find the object
                Ray ray = new Ray(transform.position, moveDirection);
                RaycastHit hit;

                // if it is not going to hit move
                if (!Physics.Raycast(ray, out hit, moveDistance))
                {
                    // actually move the camera/player
                    transform.position += move * playerSpeed * Time.deltaTime;
                }
            }
        }
    }
}