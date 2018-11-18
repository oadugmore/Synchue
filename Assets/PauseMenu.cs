using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour 
{
	bool paused = false;
	Animator menuAnim;

	const string menuSceneName = "Menu";

	// Use this for initialization
	void Start () 
	{
		menuAnim = GetComponentInChildren<Animator>();
	}

	public void PauseButtonPressed()
	{
		paused = !paused;
		menuAnim.SetBool("Paused", paused);
		if (paused)
		{
			Time.timeScale = 0f;
			
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void RestartButtonPressed()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void MenuButtonPressed()
	{
		Time.timeScale = 1f;
		SceneManager.LoadScene(menuSceneName);
	}
	
}
