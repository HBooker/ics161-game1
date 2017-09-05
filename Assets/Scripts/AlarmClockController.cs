using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClockController : MonoBehaviour {

	float timeToActivate;
	public float blinkFrequency = 1.0f;

	enum states {DORMANT, ACTIVE, INACTIVE};
	states state = states.INACTIVE;
	float elapsedTime = 0.0f;
	AudioSource audioTicking;
	AudioSource audioRinging;
	Animator animRinging;
	int animRotate = 0;
	bool active = false;

	public bool StateInactive()
	{
		return state == states.INACTIVE;
	}

	// Use this for initialization
	void Start () {
		AudioSource[] audioSources = GetComponents<AudioSource> ();

		if (audioSources [0].clip.name == "Ticking Clock") {
			audioTicking = audioSources [0];
			audioRinging = audioSources [1];
		} else {
			audioTicking = audioSources [1];
			audioRinging = audioSources [0];
		}

		audioTicking.enabled = false;
		audioRinging.enabled = false;
		animRinging = GetComponent<Animator> ();
		animRinging.enabled = false;
	}

	private void EnableRinging()
	{
		state = states.ACTIVE;
		audioTicking.enabled = false;
		audioRinging.enabled = true;
		animRinging.enabled = true;
	}

	private void DisableRinging()
	{
		state = states.INACTIVE;
		audioRinging.enabled = false;
		animRinging.enabled = false;
	}

	void SetActivationTime(float at)
	{
		timeToActivate = at;
		state = states.DORMANT;
		elapsedTime = 0.0f;
	}

	public void ActivateClock()
	{
		audioTicking.enabled = true;
		active = true;
	}

	void OnHit()
	{
		GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None;

		switch(state)
		{
		case states.DORMANT: //clock is hit before it rings
			state = states.INACTIVE;
			audioTicking.enabled = false;
			FindObjectOfType<ScoreController> ().ResetStreak();
			break;
		case states.ACTIVE: //clock is hit while ringing
			DisableRinging();
			FindObjectOfType<ScoreController> ().AddPoints(1.0f / (1.0f + elapsedTime - timeToActivate));
			break;
		case states.INACTIVE: //clock is hit again after already being disabled
			FindObjectOfType<ScoreController> ().ResetStreak();
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!active)
			return;
		
		elapsedTime += Time.deltaTime;

		switch (state)
		{
		case states.DORMANT:
			if (elapsedTime >= timeToActivate)
				EnableRinging();
			break;
		case states.ACTIVE:
			break;
		case states.INACTIVE:
			break;
		}
	}
}
