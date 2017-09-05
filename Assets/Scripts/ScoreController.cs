using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {
	int currentScore = 0;
	string playerName = "";
	public int scorePerClock = 100;
	UnityEngine.UI.Text scoreText;
	int streakMultiplier = 1;
	public ScoreSystem scoreboard;
	bool gameOver = false;

	GameObject highScoreElement;

	// Use this for initialization
	void Start () {
		scoreText = GameObject.FindGameObjectWithTag ("Score Counter").GetComponent<UnityEngine.UI.Text>();
		highScoreElement = GameObject.FindGameObjectWithTag ("High Score");

		highScoreElement.SetActive (false);
	}

	public void ResetStreak() {
		streakMultiplier = 1;
	}

	public void AddPoints(float timeMultiplier)
	{
		int scoreToAdd = scorePerClock + (int)(scorePerClock * timeMultiplier) * streakMultiplier;
		currentScore += scoreToAdd;
		streakMultiplier++;

	}
	 
	public void EndOfGame()
	{
		gameOver = true;
		if(currentScore > 0 && scoreboard.IsNewHighScore(currentScore)) 
		{
			highScoreElement.SetActive (true);
		}
	}

	public void SetPlayerName()
	{
		playerName = highScoreElement.GetComponentInChildren<UnityEngine.UI.InputField>().text;
	}

	public void CommitScore()
	{
		scoreboard.AddNewScore (currentScore, playerName);
	}

	void OnDestroy()
	{
		if(currentScore > 0 && gameOver)
			CommitScore ();
	}

	// Update is called once per frame
	void Update () {
		scoreText.text = "Score: " + currentScore + "\nMult: " + streakMultiplier + "x";
	}
}