using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemies : MonoBehaviour
{

	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Backspace) && Input.GetKeyDown(KeyCode.Equals) && Input.GetKeyDown(KeyCode.Minus))
		{
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			
			for(int i = 0; i < enemies.GetLength(0); i++)
			{
				Destroy(enemies[i]);
			}
		}
	}
}
