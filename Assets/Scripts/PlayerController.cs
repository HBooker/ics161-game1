using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	string fireButton = "Fire1";
	public float shotForce = 1000.0f;
	public bool fireEnabled = true;
	AudioSource fireSound;
	ScoreController score;
	Animation fireAnim;

	void Start () {
		fireSound = GetComponentInChildren<AudioSource> ();
		score = FindObjectOfType<ScoreController> ();
		fireAnim = GetComponentInChildren<Animation> ();
	}

	void Fire() {
		RaycastHit shot;
		GameObject hitObject;

		fireSound.Play ();
		fireAnim.Play ();

		if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out shot))
		{
				hitObject = shot.rigidbody.gameObject;
				hitObject.GetComponent<Rigidbody>().AddForce(transform.forward * shotForce);
				hitObject.SendMessage ("OnHit", SendMessageOptions.DontRequireReceiver);
		}
		else
		{
			score.ResetStreak ();
		}
	}


	void Update () {
		if(fireEnabled && Input.GetButtonDown(fireButton))
		{
			Fire ();
		}
	}
}
