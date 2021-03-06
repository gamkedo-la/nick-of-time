﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupSinglePlayerMode : MonoBehaviour
{
	void Start ()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	void Update ()
	{
		if(GameManager.singleGame)
			setup();

		Destroy(gameObject);
	}
	
	void setup()
	{
		GameObject[] inCameraObjects = GameObject.FindGameObjectsWithTag("InCamera");
		
		for (int i = 0; i < inCameraObjects.GetLength(0); i++)
		{
			if(inCameraObjects[i] != null
			&& inCameraObjects[i].name != "HPBar1"
			&& inCameraObjects[i].name != "Canvas_JointCamera"
			&& inCameraObjects[i].name != "P1"
			&& inCameraObjects[i].name != "ActionBar1")
				Destroy(inCameraObjects[i]);
		}
		
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		for(int i = 0; i < players.GetLength(0); i++)
		{
			if (players[i].name != "Player1" && players[i].transform.parent.name != "Players1")
			{
				Destroy(players[i]);
			}
		}

		GameObject[] player2OnlyObjects = GameObject.FindGameObjectsWithTag("Player2Only");
		for (int i = 0; i < player2OnlyObjects.Length; i++)
			Destroy(player2OnlyObjects[i]);
		
		GameObject lightingAndBlocks = GameObject.Find("LightingAndBlocks");
		if (lightingAndBlocks != null)
		{
			Transform[] tutorials = lightingAndBlocks.GetComponentsInChildren<Transform>(true);

			for (int i = 1; i < tutorials.Length; i++)
			{
				if (tutorials[i].CompareTag("Player2Only"))
				{
					Destroy(tutorials[i].gameObject);
				}
			}
		}
	}
}
