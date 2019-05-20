using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
	
	public string playerToPlayerHitTag = "PlayerAttack";
	
	[HideInInspector] static public int triggerCount = 0;
	
	void Awake () {
		triggerCount = 0;
	}
	
	void Start() {
		if(!TogglesValues.coop)
		{
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			string enemyHitTag = players[0].GetComponent<HitCheck>().hitTags[0];
			
			for(int i = 0; i < players.GetLength(0); i++)
			{
				players[i].GetComponent<HitCheck>().hitTags = new string[2];
				
				players[i].GetComponent<HitCheck>().hitTags[0] = enemyHitTag;
				players[i].GetComponent<HitCheck>().hitTags[1] = playerToPlayerHitTag;
			}
		}
	}
	
	void Update () {
		
	}
}
