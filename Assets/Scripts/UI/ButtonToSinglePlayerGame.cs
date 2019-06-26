using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToSinglePlayerGame : MonoBehaviour {
	
	public GameObject setup;
	
	public AudioClip clickSound;
	
	private AudioSource aud = null;

	void Start ()
	{
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
	}
	
	void OnMouseOver()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if(aud != null && TogglesValues.sound)
			{
				aud.PlayOneShot(clickSound);
			}
			
			GameManager.singleGame = true;

			if (TogglesValues.story)
				SceneManager.LoadScene("Prologue");
			else
			{
				Instantiate(setup);
				SceneManager.LoadScene("Arena");
			}
		}
		
	}
}
