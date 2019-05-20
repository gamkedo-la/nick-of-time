using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnTrigger : MonoBehaviour {

	public Transform triggerTr;
	public float delay = 0.25f;
	
	public bool isCounted = true;
	public bool checkEnemies = false;
	public bool destroyEnemies = false;
	
	public GameObject objectToActivate;
	
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
		
		if(!done && timer <= 0f
		&& (!checkEnemies || GameObject.FindWithTag("Enemy") == null))
		{
			for(int c = 0; c < triggerTr.gameObject.GetComponents<CameraTriggerControl>().GetLength(0); c++)
			{
				if(triggerTr.gameObject.GetComponents<CameraTriggerControl>()[c].triggered)
				{
					if(objectToActivate)
						objectToActivate.SetActive(true);
			
					done = true;
					
					if(isCounted)
						decrementTriggerCount = 100;
					
					if(destroyEnemies)
					{
						GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
						
						for(int i = 0; i < enemies.GetLength(0); i++)
						{
							Destroy(enemies[i]);
						}
					}
				}
			}
			
			timer = delay;
		}
		
		timer -= Time.deltaTime;
	}
}
