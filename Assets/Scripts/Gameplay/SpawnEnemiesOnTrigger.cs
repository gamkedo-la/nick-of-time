using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesOnTrigger : MonoBehaviour {
	
	public Transform triggerTr;
	public float delay = 0.25f;
	
	public bool isCounted = true;
	
	public GameObject enemySpawnObject;
	
	private float timer = 0f;
	
	private bool done = false;
	private int decrementTriggerCount = -1;
	
	void Start () {
		if(isCounted)
			LevelManager.triggerCount++;
		
		done = false;
		
		timer = delay;
	}
	
	void Update () {
		
		if(decrementTriggerCount == 0)
		{
			LevelManager.triggerCount--;
			decrementTriggerCount = -1;
		}
		else if(decrementTriggerCount > 0)
		{
			decrementTriggerCount--;
		}
		
		if(!done && timer <= 0f)
		{
			for(int c = 0; c < triggerTr.gameObject.GetComponents<CameraTriggerControl>().GetLength(0); c++)
			{
				if(triggerTr.gameObject.GetComponents<CameraTriggerControl>()[c].triggered)
				{
					for(int i = 0; i < triggerTr.transform.childCount; i++)
					{
						Instantiate(enemySpawnObject, triggerTr.transform.GetChild(i).transform.position, Quaternion.Euler(0f,0f,0f));
					}
			
					done = true;
					
					if(isCounted)
						decrementTriggerCount = 100;
				}
			}
			
			timer = delay;
		}
		
		timer -= Time.deltaTime;
	}
}
