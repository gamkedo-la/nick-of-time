using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicGameOverStateCompletion : MonoBehaviour {
	
	public GameObject fadeOutObject;
	
	public AudioClip deathSound;
	
	private AudioSource aud;
	
	void Start()
	{
		aud = GetComponent<AudioSource>();
		if(aud == null)
		{
			AudioSource[] auds = FindObjectsOfType<AudioSource>();
			
			for(int i = 0; i < auds.GetLength(0); i++)
			{
				if(auds[i].gameObject.GetComponent<KeepInBetweenScenes>() != null)
				{
					aud = auds[i];
					break;
				}
			}
		}
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
	}

	public void fadeToMenu()
	{
		fadeOutObject.GetComponent<PublicSceneChangeFunction>().sceneName = "Menu";
		Instantiate(fadeOutObject);
	}
	
	public void playDeathSound()
	{
		if(TogglesValues.sound)
			aud.PlayOneShot(deathSound);
	}
}
