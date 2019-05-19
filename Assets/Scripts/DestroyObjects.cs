using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjects : MonoBehaviour {
	
	public float delay = -1f;
	public bool selfDestroy = false;
	
	public GameObject[] objects;
	
	private bool done = false;
	
	void Awake () {
		if(delay <= -1)
		{
			for(int i = 0; i < objects.GetLength(0); i++)
			{
				Destroy(objects[i]);
			}
			
			if(selfDestroy) Destroy(gameObject);
			
			done = true;
		}
	}
	
	void Start() {
		if(!done)
		{
			if(delay <= 0f)
			{
				for(int i = 0; i < objects.GetLength(0); i++)
				{
					Destroy(objects[i]);
				}
				
				if(selfDestroy) Destroy(gameObject);
			
				done = true;
			}
		}
	}
	
	void Update () {
		if(!done)
		{
			if(delay <= 0f)
			{
				for(int i = 0; i < objects.GetLength(0); i++)
				{
					Destroy(objects[i]);
				}
				
				if(selfDestroy) Destroy(gameObject);
			
				done = true;
			}
			
			delay -= Time.deltaTime;
		}
	}
}
