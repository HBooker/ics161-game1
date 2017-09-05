using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour {

	public KeyCode menuKey	 = KeyCode.Escape;

	SceneController scene;
	bool menuEnabled;
	GameObject[] buttons;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpc;
	PlayerController pc;

	void Start () {
		buttons = GameObject.FindGameObjectsWithTag ("Pause Menu Button");
		pc = FindObjectOfType<PlayerController>();
		scene = FindObjectOfType<SceneController> ();
		menuEnabled = true;
		ToggleMenu ();
	}

	public void ToggleMenu()
	{
		menuEnabled = !menuEnabled;
		fpc.mouseLookEnabled = !menuEnabled;
		fpc.m_MouseLook.SetCursorLock (!menuEnabled);
		pc.fireEnabled = !menuEnabled;

		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Clock");

		foreach(GameObject go in gos)
		{
			AudioSource[] ads = go.GetComponentsInChildren<AudioSource> ();

			foreach(AudioSource ad in ads)
			{
				ad.mute = menuEnabled;
			}
		}

		if (menuEnabled) {
			scene.PauseScene ();
		} else {
			scene.ResumeScene ();
		}

		foreach(GameObject button in buttons)
		{
			button.SetActive (menuEnabled);
		}
	}

	void Update () {
		if(Input.GetKeyDown(menuKey))
		{
			ToggleMenu ();
		}
	}
}
