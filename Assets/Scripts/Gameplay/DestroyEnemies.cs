using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemies : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetButtonDown("Debug2"))
		{
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			
			for(int i = 0; i < enemies.GetLength(0); i++)
			{
				Destroy(enemies[i]);
			}
		}
	}
}
