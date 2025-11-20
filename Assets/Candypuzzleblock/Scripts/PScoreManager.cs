using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PScoreManager : PSingleton<PScoreManager> 
{
	[SerializeField] private Text txtScore,highScore;
	[SerializeField] private GameObject scoreAnimator;
	[SerializeField] private Text txtAnimatedText;
	private int Score = 0;
	
	private int targetScore = 0;

	void Start()
	{
		targetScore = LevelDataController.instance.Level * 2000;
		txtScore.text = Score.ToString ();	
        highScore.text = PlayerPrefs.GetInt("BestScore_" + PGameController.gameMode.ToString()).ToString ();  
	}

	public void AddScore(int scoreToAdd, bool doAnimate = true)
	{
		int oldScore = Score;
		Score += scoreToAdd;

		StartCoroutine (SetScore(oldScore, Score));

		if (doAnimate) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePos.z = 0;
			scoreAnimator.transform.position = mousePos;
			txtAnimatedText.text = "+" + scoreToAdd.ToString ();
			scoreAnimator.SetActive (true);
		}

		if (Score >= targetScore)
		{
			StartCoroutine(PGameController.Instance.WinGame());
		}
	}

	public int GetScore()
	{
		return Score;
	}

	IEnumerator SetScore(int lastScore, int currentScore)
	{
		int IterationSize = (currentScore - lastScore) / 10;

		for (int index = 1; index < 10; index++) {
			lastScore += IterationSize;
			txtScore.text =  string.Format("{0:#,#.}", lastScore);
			yield return new WaitForEndOfFrame ();
		}
		txtScore.text =  string.Format("{0:#,#.}", currentScore);
		yield return new WaitForSeconds (1F);
		scoreAnimator.SetActive (false);
	}
}
