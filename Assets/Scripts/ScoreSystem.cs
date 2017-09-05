using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[CreateAssetMenu]
public class ScoreSystem : ScriptableObject {
	int scoreboardLength = 10;
	SortedList<int, string> scoreboard;
	string filepath;

	void Awake()
	{
		scoreboard = new SortedList<int, string> (new ScoreSorter());

		for(int i = 1; i <= 10; ++i)
		{
			AddNewScore (10000 + i * 1000, "" + (i-1) + "" + (i-1) + "" + (i-1));
		}
	}

	public void SaveScoreboard()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (filepath);

		ScoreData data = new ScoreData ();
		data.scoreboard = scoreboard;

		bf.Serialize (file, data);
		file.Close ();
	}

	public void LoadScoreboard()
	{
		if(!File.Exists(filepath))
		{
			return;
		}

		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (filepath, FileMode.Open);
		ScoreData data = (ScoreData)bf.Deserialize (file);
		file.Close ();
		scoreboard = data.scoreboard;
	}

	void OnEnable()
	{
		filepath = Application.persistentDataPath + "/scoreboard_storage.dat";
		LoadScoreboard ();

		if(scoreboard == null)
		{
			scoreboard = new SortedList<int, string> (new ScoreSorter());
		}
	}

	void OnDisable()
	{
		SaveScoreboard ();
	}

	public IList<int> GetScoreValues()
	{
		return scoreboard.Keys;
	}

	public IList<string> GetPlayerIds()
	{
		return scoreboard.Values;
	}
		
	int GetLowestScore()
	{
		return scoreboard.Keys[scoreboardLength - 1];
	}

	void ReplaceLowestScoreWith(int score, string playerId)
	{
		scoreboard.RemoveAt (scoreboardLength - 1);
		scoreboard.Add (score, playerId);
	}

	bool BoardIsFull()
	{
		return scoreboard.Count >= scoreboardLength;
	}

	public bool IsNewHighScore(int score)
	{
		return (!BoardIsFull() || score >= GetLowestScore());
	}

	public bool AddNewScore(int score, string playerId)
	{
		if(!BoardIsFull())
		{
			scoreboard.Add (score, playerId);
		}
		else if(score >= GetLowestScore())
		{
			ReplaceLowestScoreWith (score, playerId);
		}
		else
		{
			return false;
		}

		return true;
	}
}

[Serializable]
class ScoreData
{
	public SortedList<int, string> scoreboard;
}

[Serializable]
class ScoreSorter : IComparer<int>
{
	public int Compare(int x, int y)
	{
		return y - x;
	}
}