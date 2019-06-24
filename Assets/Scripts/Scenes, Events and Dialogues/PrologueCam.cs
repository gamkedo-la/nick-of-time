using System.Collections;
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
		if(aud != null && TogglesValues.music && clip1 != null)
			aud.PlayOneShot(clip1, 0.3f);
	}
	
	public void playAudioClip2()
	{
		if (aud != null && TogglesValues.music && clip2 != null)
		{
			aud.PlayOneShot(clip2);
			aud.pitch = 1.385f;
		}
	}

	public void playDoorSound()
	{
		if (aud != null && TogglesValues.sound && door != null)
			aud.PlayOneShot(door, 0.5f);
	}

	public void stopAudioClip()
	{
		if(aud != null)
			aud.Stop();
	}
	
	public void volumeTo(float vol)
	{
		if(aud != null)
			aud.volume = vol;
	}
	
	public void finishPrologue()
	{
		stopAudioClip();
		volumeTo(1f);
		aud.pitch = 1f;

		SceneManager.LoadScene(sceneName);
		
		if(GameManager.singleGame)
			Instantiate(singlePlay_setup);
	}
	
}
