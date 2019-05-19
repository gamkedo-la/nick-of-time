using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonToDoublePlayerGame : MonoBehaviour {
	
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
			
			GameManager.singleGame = false;
			
			if(TogglesValues.story)
				SceneManager.LoadScene("Prologue");
			else
				SceneManager.LoadScene("Arena");
			
		}
		
	}
}
