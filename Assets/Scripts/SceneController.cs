using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeScene(string sceneName)
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (sceneName);
	}

	public void QuitGame()
	{
		Application.Quit ();
	}

	public void PauseScene()
	{
		Time.timeScale = 0.0f;
	}

	public void ResumeScene()
	{
		Time.timeScale = 1.0f;
	}
}
