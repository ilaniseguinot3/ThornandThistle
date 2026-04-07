using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
	public string GameScene;
	public string CreditsScene;
	public string StartScene;

	public void LoadTheGame()
	{
		SceneManager.LoadScene(GameScene);
	}

	public void LoadTheCredits()
	{
		SceneManager.LoadScene(CreditsScene);
	}

	public void LoadStartScene()
	{
		SceneManager.LoadScene(StartScene);
	}
}
