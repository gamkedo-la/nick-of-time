﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrologueCam : MonoBehaviour {
	
	public GameObject madClocks;
	public GameObject title;
	public GameObject singlePlay_setup;
	
	[Space]
	public AudioClip clip1;
	public AudioClip clip2;
	public AudioClip door;
	
	[Space]
	public string sceneName = "Play";
	
	private AudioSource aud;
	
	void Start () {
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
	
	void Update () {
		
	}
	
	public void enableMadClocks() {
		madClocks.SetActive(true);
	}
	
	public void disableMadClocks() {
		madClocks.SetActive(false);
	}
	
	public void enableTitle() {
		title.SetActive(true);
	}
	
	public void playAudioClip1()
	{
		if(TogglesValues.music && clip1 != null)
			aud.PlayOneShot(clip1, 0.3f);
	}
	
	public void playAudioClip2()
	{
		if(TogglesValues.music && clip2 != null)
			aud.PlayOneShot(clip2);
	}

	public void playDoorSound()
	{
		if (TogglesValues.sound && door != null)
			aud.PlayOneShot(door, 0.5f);
	}

	public void stopAudioClip()
	{
		aud.Stop();
	}
	
	public void volumeTo(float vol)
	{
		aud.volume = vol;
	}
	
	public void finishPrologue() {
		SceneManager.LoadScene(sceneName);
		
		if(GameManager.singleGame)
			Instantiate(singlePlay_setup);
	}
	
}
