using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToScene : MonoBehaviour {
	
	public string sceneName = "null";
	
	public bool startTime = false;
	
	public AudioClip clickSound;
	
	private AudioSource aud = null;
	
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
	
	void OnMouseOver() {
		if(Input.GetMouseButtonDown(0))
		{
			if(aud != null && TogglesValues.sound)
			{
				aud.PlayOneShot(clickSound);
			}
			
			if(sceneName == "quit" || sceneName == "Quit")
			{
				Application.Quit();
			}
			else if(sceneName != "null")
			{
				SceneManager.LoadScene(sceneName);
				
				if(startTime) Time.timeScale = 1f;
			}
		}	
	}

	public void OnSelection()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		if (sceneName == "quit" || sceneName == "Quit")
		{
			Application.Quit();
		}
		else if (sceneName != "null")
		{
			SceneManager.LoadScene(sceneName);

			if (startTime) Time.timeScale = 1f;
		}
	}
}
