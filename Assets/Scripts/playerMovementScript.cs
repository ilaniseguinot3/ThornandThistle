using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class playerMovementScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            // Lock the cursor to the center of the screen
            Cursor.lockState = CursorLockMode.Locked;
    }

     
    // set default speeds
    float horizontalSpeed = 0.5f;
    float verticalSpeed = 0.5f;
    int playerSpeed = 500;

    // save yaw and pitch (camera rotation)
    float yaw = 0f;
    float pitch = 0f;

    // Update is called once per frame
    void Update()
    {        
        // check if mouse has moved, if it has, rotate the camera and update yaw and pitch
        if (Mouse.current == null)
            return;
        else
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * horizontalSpeed;
            pitch -= mouseDelta.y * verticalSpeed;

            // clamp pitch so camera cannot invert
            pitch = Mathf.Clamp(pitch, -89f, 89f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }

        
        // get current key pressed
        var key = Keyboard.current;

        // if nothing is being pressed, return
        if (key == null)
            return;
        // otherwise move accordingly
        else 
        {
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

            // actually move the camera/player
            transform.position += move * playerSpeed * Time.deltaTime;
        }
    }
}
