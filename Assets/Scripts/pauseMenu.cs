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
    public string TutorialScene;

    //other variables
    public GameObject tutorialCanvas;
    public GameObject pauseCanvas;
    private bool open;
    public GameObject crosshairs;
    public playerMovementScript playerMovementMouse;

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
        
        open = !open;
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        // Show crosshairs
        crosshairs.SetActive(true);
        playerMovementMouse.activeMouse = true;
        pauseCanvas.SetActive(false);
    }

    public void closeTutorial()
    {
        print("resume");
        // lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Show crosshairs
        crosshairs.SetActive(true);
        playerMovementMouse.activeMouse = true;
        tutorialCanvas.SetActive(false);
    }

    public void playTutorial()
    {
        SceneManager.LoadScene(TutorialScene);
    }

    public void quitTheGame()
    {
        Application.Quit();
    }

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
