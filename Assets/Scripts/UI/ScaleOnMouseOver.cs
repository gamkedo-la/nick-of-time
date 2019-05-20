using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnMouseOver : MonoBehaviour {
	
	public Vector3 newScale = new Vector3(1.2f, 1.2f, 1.2f);
	
	public AudioClip hoverSound;
	
	private AudioSource aud = null;
	
	private Vector3 previousScale = new Vector3(1f, 1f, 1f);
	
	private bool doScale = false;
	
	void Start () {
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
		
		previousScale = transform.localScale;
	}
	
	void Update () {
		if(doScale)
		{
			transform.localScale = newScale;
		}
		else
		{
			transform.localScale = previousScale;
		}
		
		doScale = false;
	}
	
	void OnMouseOver() {
		if(transform.localScale != newScale && aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(hoverSound);
		}
		doScale = true;
	}
}
