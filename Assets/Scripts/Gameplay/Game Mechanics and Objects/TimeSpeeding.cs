using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSpeeding : MonoBehaviour {
	
	public float timeSpeed = 4f;

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetButtonDown("Debug1"))
		{
			if(Time.timeScale <= 1f)
				Time.timeScale = timeSpeed;
			else
				Time.timeScale = 1f;
		}
	}
}
