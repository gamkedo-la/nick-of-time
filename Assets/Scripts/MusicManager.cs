using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
	
	public AudioClip menuMusic;
	public AudioClip arenaMusic;
	
	private AudioSource aud;
	
	private string prevScene;
	

	void Start () {
		aud = GetComponent<AudioSource>();
	}
	
	void Update () {
		
		string newScene = SceneManager.GetActiveScene().name;
		
		if((newScene == "Menu"
			|| newScene == "Help"
			|| newScene == "Credits"
			|| newScene == "Toggles")
			&& !aud.isPlaying
			&& TogglesValues.music)
				newScene = "MusicToggle";
		
		if(newScene != prevScene || !aud.isPlaying)
			changeMusicState(newScene);
		
		prevScene = newScene;
	}
	
	public void changeMusicState(string newScene)
	{
		if(
				((newScene == "Menu"
				|| newScene == "Help"
				|| newScene == "Credits"
				|| newScene == "Toggles")
				&&
				(prevScene != "Menu"
				&& prevScene != "Help"
				&& prevScene != "Credits"
				&& prevScene != "Toggles"))
				
				//For turning on music when music toggles to on AND for repeating music
				|| newScene == "MusicToggle"
				)
			{
				aud.Stop();
		
				if(!aud.isPlaying)
				{
					if(aud != null && TogglesValues.music)
						aud.PlayOneShot(menuMusic);
				}
			}
			else if(newScene== "Arena"
		
				//TEMPORARILY
				|| newScene == "Play")
		
			{
				aud.Stop();
			
				if(!aud.isPlaying)
				{
					if(aud != null && TogglesValues.music)
						aud.PlayOneShot(arenaMusic);
				}
			}
			else if(newScene == "Prologue")
			{
				aud.Stop();
			}
	}
}
