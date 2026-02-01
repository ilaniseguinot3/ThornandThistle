using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovementScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int playerSpeed = 1;
        var key = Keyboard.current;
        if (key == null)
            return;
        else 
        {
            if(key.wKey.isPressed)
                this.transform.Translate(0, 0, -1 * playerSpeed);
            if(key.sKey.isPressed)
                this.transform.Translate(0, 0, 1 * playerSpeed);
            if(key.aKey.isPressed)
                this.transform.Translate(-1 * playerSpeed, 0, 0);
            if(key.dKey.isPressed)
                this.transform.Translate(1 * playerSpeed, 0, 0);
        }
    }
}
