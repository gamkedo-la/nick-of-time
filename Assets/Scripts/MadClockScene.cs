using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadClockScene : MonoBehaviour {
	
	public GameObject madClock;
	
	public float delay = 0.01f;
	
	public float maxDistance = 1f;
	public float minDistance = -1f;
	
	public float zMinDistance = 0.4f;
	public float zMaxDistance = 1f;
	
	public float maxScale = 10f;
	
	private float timer = 0f;

	void Start () {
		
	}
	
	void Update () {
		
		if(timer <= 0f)
		{
			GameObject instance = Instantiate(madClock,
			Camera.main.transform.position, Quaternion.Euler(0f,0f,0f));
			
			instance.transform.position += new Vector3( 
				Random.Range(minDistance,maxDistance),
				Random.Range(minDistance,maxDistance),
				Random.Range(zMinDistance,zMaxDistance)
			);
			
			float scaleInc = Random.Range(0f, maxScale);
			
			instance.transform.localScale += new Vector3( scaleInc, scaleInc, 0f );
			
			timer = delay;
		}
		
		timer -= Time.deltaTime;
	}
}
