using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArenaScoreSystem : MonoBehaviour
{
	[SerializeField] private int score = 0;
	[SerializeField] private int weaponScore = 0;

	[SerializeField] private int changeWeaponAtWeaponScore = 1000;

	public GameObject player;

	private TextMeshProUGUI scoreText;

	static private ArenaScoreSystem pl1Score1 = null;
	static private ArenaScoreSystem pl2Score1 = null;

	static private ArenaScoreSystem pl1Score2 = null;
	static private ArenaScoreSystem pl2Score2 = null;

	static public void AddPlayer1Score(int sc)
	{
		if (pl1Score1 != null)
		{
			pl1Score1.score += sc;
			pl1Score1.weaponScore += sc;
		}
	}

	static public void AddPlayer2Score(int sc)
	{
		if (pl2Score1 != null)
		{
			pl2Score1.score += sc;
			pl2Score1.weaponScore += sc;
		}
		if (pl1Score2 != null)
		{
			pl2Score2.score = pl2Score1.score;
			pl2Score2.weaponScore = pl2Score1.score;
		}
	}

	void Start()
    {
		score = 0;
		weaponScore = 0;

		if (player.name == "Player1")
		{
			if (!pl1Score1) pl1Score1 = this;
			else pl1Score2 = this;
		}
		else if (player.name == "Player2")
		{
			if (!pl2Score1) pl2Score1 = this;
			else pl2Score2 = this;
		}

		scoreText = GetComponent<TextMeshProUGUI>();
    }
	
    void Update()
    {
		scoreText.text = "Score: " + score.ToString();

		if (this == pl1Score1 || this == pl2Score1)
		{
			if (weaponScore >= changeWeaponAtWeaponScore)
			{
				int prevWeaponId = player.GetComponent<PlayerController>().weaponPossession.weaponID;

				player.GetComponent<PlayerController>().weaponPossession.weaponID = Random.Range(0, 4);

				if (player.GetComponent<PlayerController>().weaponPossession.weaponID == prevWeaponId)
				{
					if (prevWeaponId > 0) player.GetComponent<PlayerController>().weaponPossession.weaponID--;
					else if (prevWeaponId < 3) player.GetComponent<PlayerController>().weaponPossession.weaponID++;
				}

				weaponScore -= changeWeaponAtWeaponScore;

				if (this == pl1Score1)
					Subtitles.AddPlayer1Subtitle("Weapon Changed!");
				else if (this == pl2Score1)
					Subtitles.AddPlayer2Subtitle("Weapon Changed!");
			}
		}

		if (pl1Score1 != null)
		{
			pl1Score2.score = pl1Score1.score;
			pl1Score2.weaponScore = pl1Score1.score;
		}

		if (pl2Score1 != null)
		{
			pl2Score2.score = pl2Score1.score;
			pl2Score2.weaponScore = pl2Score1.score;
		}
	}
}
