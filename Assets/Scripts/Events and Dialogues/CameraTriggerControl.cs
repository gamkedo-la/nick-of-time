using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTriggerControl : MonoBehaviour {
	
	public Camera cam;
	public string objectTag = "Player";
	public string objectName = "null";
	
	[HideInInspector] public bool triggered = false;
	
	void Start () {
		
	}
	
	void Update () {
		if(cam == null || cam.GetComponent<LerpToTransform>().enabled == false)
			Destroy(this);
	}
	
	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.CompareTag(objectTag) && (coll.gameObject.name == "null" || coll.gameObject.name == objectName) )
		{
			cam.GetComponent<LerpToTransform>().tr = gameObject.transform;
			triggered = true;
		}
	}
	
	//To set the transform follow back to Player after getting out of the trigger zone
	/*
	void OnTriggerExit2D(Collider2D coll)
	{
		if(coll.gameObject.CompareTag("Player"))
		{
			Camera.main.GetComponent<LerpToTransform>().tr = coll.gameObject.transform;
			triggered = false;
		}
	}
	*/
}
