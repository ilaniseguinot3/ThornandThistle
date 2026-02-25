using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
	public string Scene;

	public void LoadTheScene()
	{
		SceneManager.LoadScene(Scene);
	}
}
