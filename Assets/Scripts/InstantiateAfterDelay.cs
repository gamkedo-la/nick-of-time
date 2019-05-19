using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateAfterDelay : MonoBehaviour {
	
	public GameObject objectToInstantiate;
	public int objectInstantiateCycles = 1;
	
	public float delay = 0.5f;
	
	public float destroyDelay = 1000f;
	
	public bool atDefaultPosition = false;
	
	private float timer = 0f;
	
	void Start () {
		timer = delay;
	}
	
	void Update () {
		if(timer <= 0f && objectInstantiateCycles > 0)
		{
			if(atDefaultPosition)
			{
				Instantiate(objectToInstantiate);
			}
			else
			{
				Instantiate(objectToInstantiate, transform.position, Quaternion.Euler(0f,0f,0f));
			}
			
			objectInstantiateCycles--;
			timer = delay;
		}
		
		timer -= Time.deltaTime;
		
		if(destroyDelay <= 100f)
		{
			if(destroyDelay <= 0f)
			{
				Destroy(gameObject);
			}
			
			destroyDelay -= Time.deltaTime;
		}
	}

	public void destroy()
	{
		Destroy(gameObject);
	}
}
