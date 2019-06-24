using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraTriggerControl : MonoBehaviour
{
	public Camera cam;

	[Space]
	public string objectTag = "Player";
	public string objectName = "null";

	[Space]
	public float size;

	[Space]
	public LightLerp moodAmbianceLerper;
	public Color ambientColor;
	public float ambientIntensity;
	
	[HideInInspector] public bool triggered = false;

	static private CameraTriggerControl prevContForPl1 = null;
	static private CameraTriggerControl prevContForPl2 = null;

	void Start ()
	{
		
	}
	
	void Update ()
	{
		if(cam == null || cam.GetComponent<LerpToTransform>().enabled == false)
			Destroy(this);
	}

	void TriggerSetup()
	{
		if (objectName == "Player1")
		{
			if (prevContForPl1 != null)
			{
				if (prevContForPl1 != this && prevContForPl2 != this)
					prevContForPl1.triggered = false;
			}
			
			prevContForPl1 = this;
		}
		else if (objectName == "Player2")
		{
			if (prevContForPl2 != null)
			{
				if (prevContForPl2 != this && prevContForPl1 != this)
					prevContForPl2.triggered = false;
			}
			
			prevContForPl2 = this;
		}

		triggered = true;
	}

	void ControlStart()
	{
		cam.GetComponent<LerpToTransform>().tr = gameObject.transform;
		cam.GetComponent<LerpToCamSize>().size = size;

		moodAmbianceLerper.color = ambientColor;
		moodAmbianceLerper.intensity = ambientIntensity;
		moodAmbianceLerper.gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, moodAmbianceLerper.gameObject.transform.position.z);

		TriggerSetup();
	}
	
	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag(objectTag) && (coll.gameObject.name == "null" || coll.gameObject.name == objectName))
			ControlStart();
	}

	void OnTriggerStay2D(Collider2D coll)
	{
		if (coll.gameObject.CompareTag(objectTag) && (coll.gameObject.name == "null" || coll.gameObject.name == objectName))
			TriggerSetup();
	}
}
