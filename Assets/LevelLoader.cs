using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void LoadLevel(int levelNumber)
	{
		string sceneName = "Level" + levelNumber;
		SceneManager.LoadScene(sceneName);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
