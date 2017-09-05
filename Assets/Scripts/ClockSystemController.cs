using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockSystemController : MonoBehaviour 
{
	int numClocks;
	float initialTimeOffset = 7.0f;
	float timeDelta = 4.0f;
	AlarmClockController[] clocks;
	bool roundStarted = false;



	public GameObject gameOverCanvas;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpc;

	void Awake()
	{
		gameOverCanvas.SetActive (false);
	}

	void Start()
	{
		clocks = FindObjectsOfType<AlarmClockController>();
		numClocks = clocks.Length;
		DisableColliders ();
		SetActivationTimes ();
	}

	void EnableColliders()
	{
		foreach(AlarmClockController clock in clocks)
		{
			clock.GetComponent<BoxCollider> ().enabled = true;
			clock.GetComponent<Rigidbody> ().useGravity = true;
		}
	}

	void DisableColliders()
	{
		foreach(AlarmClockController clock in clocks)
		{
			clock.GetComponent<BoxCollider> ().enabled = false;
			clock.GetComponent<Rigidbody> ().useGravity = false;
		}
	}

	void SetActivationTimes()
	{
		List<int> clockPool = new List<int>();
		float timeOffset = initialTimeOffset;

		for (int i = 0; i < numClocks; ++i) 
		{
			clockPool.Add (i);
		}

		for (int i = 0; i < numClocks; ++i) 
		{
			int nextClock = clockPool [(int)(Random.value * clockPool.Count)];
			float nextTime;
			nextTime = i + 1;
			nextTime /=  Mathf.Ceil((i + 1) / 8.0f);
			nextTime *= timeDelta;
			nextTime /= (i + 1);


			timeOffset += nextTime;
			clocks[nextClock].SendMessage ("SetActivationTime", timeOffset);
			clockPool.Remove (nextClock); 
		}
	}

	void StartRound () 
	{
		roundStarted = true;
		GameObject.FindGameObjectWithTag ("Start Prompt").SetActive(false);
		GameObject.FindGameObjectWithTag ("Score Counter").GetComponent<UnityEngine.UI.Text> ().enabled = true;
		EnableColliders ();

		foreach (AlarmClockController clock in clocks) {
			clock.ActivateClock ();
		}
	}

	public bool AllClocksInactive()
	{
		foreach(AlarmClockController clock in clocks)
		{
			if (!clock.StateInactive ())
				return false;
		}

		return true;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!roundStarted && (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
			StartRound ();
		}

		if(AllClocksInactive())
		{
			FindObjectOfType<PauseMenuController> ().enabled = false;
			gameOverCanvas.SetActive (true);
			fpc.m_MouseLook.SetCursorLock (false);
			fpc.mouseLookEnabled = false;
			fpc.m_RunSpeed = 0.0f;
			fpc.m_WalkSpeed = 0.0f;
			FindObjectOfType<PlayerController> ().fireEnabled = false;
			FindObjectOfType<ScoreController> ().EndOfGame ();
		}
	}


}
