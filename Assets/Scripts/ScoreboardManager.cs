using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour {
	public ScoreSystem scoreboard;
	UnityEngine.UI.Text boardText;

	IList<int> scoreValues;
	IList<string> playerIds;

	// Use this for initialization
	void Start () {
		boardText = GetComponent<UnityEngine.UI.Text> ();

		scoreValues = scoreboard.GetScoreValues();
		playerIds = scoreboard.GetPlayerIds ();

		string newText = "High Scores\n";

		for(int i = 0; i < scoreValues.Count; ++i)
		{
			newText += "" + scoreValues [i] + "   " + (playerIds[i] == "" ? "???" : playerIds[i]) + "\n";
		}

		boardText.text = newText;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
