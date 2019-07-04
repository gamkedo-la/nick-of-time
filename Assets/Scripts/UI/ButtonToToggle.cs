using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToToggle : MonoBehaviour
{

	public bool forMusic = true;
	public bool forSound = false;
	public bool forSingleWindow = false;
	public bool forCoop = false;
	public bool forStory = false;
	public bool forSubtitles = false;

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

		UpdateState();
	}

	void UpdateState()
	{
		if (forMusic)
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(TogglesValues.music);
			gameObject.transform.GetChild(1).gameObject.SetActive(!TogglesValues.music);
		}
		else if (forSound)
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(TogglesValues.sound);
			gameObject.transform.GetChild(1).gameObject.SetActive(!TogglesValues.sound);
		}
		else if (forSingleWindow)
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(TogglesValues.singleWindow);
			gameObject.transform.GetChild(1).gameObject.SetActive(!TogglesValues.singleWindow);
		}
		else if (forCoop)
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(TogglesValues.coop);
			gameObject.transform.GetChild(1).gameObject.SetActive(!TogglesValues.coop);

			if (TogglesValues.story)
			{
				gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
				gameObject.GetComponent<ScaleOnMouseOver>().newScale = new Vector3(300f, 300f, 300f);
				TogglesValues.coop = true;
			}
			else
			{
				gameObject.GetComponent<SpriteRenderer>().color = Color.white;
				gameObject.GetComponent<ScaleOnMouseOver>().newScale = new Vector3(350f, 350f, 350f);
			}
		}
		else if (forStory)
		{
			if (TogglesValues.story)
				TogglesValues.coop = true;

			gameObject.transform.GetChild(0).gameObject.SetActive(TogglesValues.story);
			gameObject.transform.GetChild(1).gameObject.SetActive(!TogglesValues.story);
		}
		else if (forSubtitles)
		{
			gameObject.transform.GetChild(0).gameObject.SetActive(TogglesValues.subtitles);
			gameObject.transform.GetChild(1).gameObject.SetActive(!TogglesValues.subtitles);
		}
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (aud != null && TogglesValues.sound)
			{
				aud.PlayOneShot(clickSound);
			}

			if (forMusic)
			{
				TogglesValues.music = !TogglesValues.music;

				if (aud.isPlaying && !TogglesValues.music)
					aud.Stop();
				else if (TogglesValues.music)
					aud.gameObject.GetComponent<MusicManager>().changeMusicState("MusicToggle");
			}
			else if (forSound)
			{
				TogglesValues.sound = !TogglesValues.sound;
			}
			else if (forSingleWindow)
			{
				TogglesValues.singleWindow = !TogglesValues.singleWindow;
			}
			else if (forCoop && !TogglesValues.story)
			{
				TogglesValues.coop = !TogglesValues.coop;
			}
			else if (forStory)
			{
				TogglesValues.story = !TogglesValues.story;
			}
			else if (forSubtitles)
			{
				TogglesValues.subtitles = !TogglesValues.subtitles;
			}

			UpdateState();
		}
	}

	public void OnSelection()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		if (forMusic)
		{
			TogglesValues.music = !TogglesValues.music;

			if (aud.isPlaying && !TogglesValues.music)
				aud.Stop();
			else if (TogglesValues.music)
				aud.gameObject.GetComponent<MusicManager>().changeMusicState("MusicToggle");
		}
		else if (forSound)
		{
			TogglesValues.sound = !TogglesValues.sound;
		}
		else if (forSingleWindow)
		{
			TogglesValues.singleWindow = !TogglesValues.singleWindow;
		}
		else if (forCoop && !TogglesValues.story)
		{
			TogglesValues.coop = !TogglesValues.coop;
		}
		else if (forStory)
		{
			TogglesValues.story = !TogglesValues.story;
		}
		else if (forSubtitles)
		{
			TogglesValues.subtitles = !TogglesValues.subtitles;
		}

		UpdateState();
	}
}
