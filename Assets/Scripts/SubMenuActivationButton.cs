using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenuActivationButton : MonoBehaviour
{
	public GameObject[] objectsToActivate;

	public GameObject[] objectsToDeactivate;

	public bool selfDeactivate = true;

	public AudioClip clickSound;

	private AudioSource aud = null;

	void Start()
	{
		aud = GetComponent<AudioSource>();
		if (aud == null)
		{
			AudioSource[] auds = FindObjectsOfType<AudioSource>();

			for (int i = 0; i < auds.GetLength(0); i++)
			{
				if (auds[i].gameObject.GetComponent<KeepInBetweenScenes>() != null)
				{
					aud = auds[i];
					break;
				}
			}
		}
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();
	}

	void Update()
	{

	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (aud != null && TogglesValues.sound)
			{
				aud.PlayOneShot(clickSound);
			}

			for (int i = 0; i < objectsToDeactivate.GetLength(0); i++)
				objectsToDeactivate[i].GetComponent<MenuAnimatorState>().stateOFF();

			for (int i = 0; i < objectsToActivate.GetLength(0); i++)
				objectsToActivate[i].GetComponent<MenuAnimatorState>().stateON();

			if (selfDeactivate)
				gameObject.GetComponent<MenuAnimatorState>().stateOFF();
		}

	}

	public void OnSelection()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		for (int i = 0; i < objectsToDeactivate.GetLength(0); i++)
			objectsToDeactivate[i].GetComponent<MenuAnimatorState>().stateOFF();

		for (int i = 0; i < objectsToActivate.GetLength(0); i++)
			objectsToActivate[i].GetComponent<MenuAnimatorState>().stateON();

		if (selfDeactivate)
			gameObject.GetComponent<MenuAnimatorState>().stateOFF();
	}
}
