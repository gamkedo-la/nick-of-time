using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {

	public AudioClip sound;
	
	public float delay = 0f;
	
	public bool onceEverytime = false;
	
	public bool isMusic = false;
	
	public bool doRepeat = false;
	
	[HideInInspector] public bool done = false;
	
	private AudioSource aud = null;
	
	void Awake() {
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
		
		if(!done && delay <= -1f)
		{
			if(aud != null && !isMusic && TogglesValues.sound)
				aud.PlayOneShot(sound);
			else if(aud != null && isMusic && TogglesValues.music)
				aud.PlayOneShot(sound);
			
			if(!onceEverytime)
				done = true;
		}
	}
	
	void Start () {
		if(!done && delay <= 0f)
		{
			if(aud != null && !isMusic && TogglesValues.sound)
				aud.PlayOneShot(sound);
			else if(aud != null && isMusic && TogglesValues.music)
				aud.PlayOneShot(sound);
			
			if(!onceEverytime)
				done = true;
		}
	}
	
	void Update () {
		if(!done && delay <= 0f)
		{
			if(aud != null && !isMusic && TogglesValues.sound)
				aud.PlayOneShot(sound);
			else if(aud != null && isMusic && TogglesValues.music)
				aud.PlayOneShot(sound);
			
			done = true;
		}
		
		if(doRepeat && !aud.isPlaying)
		{
			if(aud != null && !isMusic && TogglesValues.sound)
				aud.PlayOneShot(sound);
			else if(aud != null && isMusic && TogglesValues.music)
				aud.PlayOneShot(sound);
		}
		
		if(aud.isPlaying && !TogglesValues.music)
			aud.Stop();
		
		delay -= Time.deltaTime;
	}
}
