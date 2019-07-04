using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeAction : MonoBehaviour {
	
	public GameObject pauseTriggerObject;
	
	public AudioClip clickSound;
	
	private AudioSource aud = null;
	
	void Start () {
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
	}
	
	void Update () {
		
	}
	
	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0))
		{
			if(aud != null && TogglesValues.sound)
			{
				aud.PlayOneShot(clickSound);
			}
			
			pauseTriggerObject.GetComponent<ActivateSingleMode>().paused = false;
			pauseTriggerObject.GetComponent<ActivateSingleMode>().pauseObject.SetActive(false);
			gameObject.transform.parent.parent.gameObject.GetComponent<PlaySound>().done = false;
			Time.timeScale = 1f;
		}
	}

	public void OnSelection()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		pauseTriggerObject.GetComponent<ActivateSingleMode>().paused = false;
		pauseTriggerObject.GetComponent<ActivateSingleMode>().pauseObject.SetActive(false);
		gameObject.transform.parent.parent.gameObject.GetComponent<PlaySound>().done = false;
		Time.timeScale = 1f;
	}
}
