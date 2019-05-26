using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetupSinglePlayerMode : MonoBehaviour {

	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	void Update () {
		if(SceneManager.GetActiveScene().name == "Story1"
		|| SceneManager.GetActiveScene().name == "Arena")
			setup();
	}
	
	void setup() {
		GameObject[] inCameraObjects = GameObject.FindGameObjectsWithTag("InCamera");
		
		for(int i = 0; i < inCameraObjects.GetLength(0); i++)
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
		
		Destroy(gameObject);
	}
}
