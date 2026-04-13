using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


public class pauseMenu : MonoBehaviour
{
    //scenes
	public string CreditsScene;
	public string StartScene;

    // changing the scenes
    public void goToStart()
	{
		SceneManager.LoadScene(StartScene);
	}

	public void LoadTheCredits()
	{
		SceneManager.LoadScene(CreditsScene);
	}

    public void resumeTheGame()
    {
        print("resume");
        open = !open;
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // Show crosshairs
        crosshairs.SetActive(true);
        playerMovementMouse.activeMouse = true;
        pauseCanvas.SetActive(false);
    }

    public void quitTheGame()
    {
        Application.Quit();
    }

    //other variables
    public GameObject pauseCanvas;
    private bool open;
    public GameObject crosshairs;
    public playerMovementScript playerMovementMouse;

    void start()
    {
        pauseCanvas.SetActive(false);
        open = false; 
    }
    // Update is called once per frame
    void Update()
    {
        // get current key pressed
            var key = Keyboard.current;
            // if nothing return
            if (key == null)
                return;

            else if (key[Key.Escape].wasPressedThisFrame)
            {
               // open the pause menu
               open = !open;  
               if (open)
               {
                   playerMovementMouse.activeMouse = false;
                   crosshairs.SetActive(false);
                   Cursor.lockState = CursorLockMode.None;
                   pauseCanvas.SetActive(true);
                   
               }       
               else
               {
                   // Lock the cursor to the center of the screen
                   Cursor.lockState = CursorLockMode.Locked;
                   // Show crosshairs
                   crosshairs.SetActive(true);
                   playerMovementMouse.activeMouse = true;
                   pauseCanvas.SetActive(false);
               }
                    
            }
    }
}
