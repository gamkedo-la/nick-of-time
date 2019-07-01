using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayMenuButtonScript : MonoBehaviour
{
	public GameObject singlePlayerSetup;

	public Color deselectedColor;
	public Color selectedColor;

	public AudioClip clickSound;

	private AudioSource aud = null;

	private GameObject defaultStoryArenaButtons;
	private GameObject doublePlayersStoryArenaButtons;
	private GameObject singleButton;
	private GameObject doubleButton;
	private GameObject p1ControlsButton;
	private GameObject p2ControlsButton;
	private GameObject playButton;

	private GameObject storyButton1;
	private GameObject storyButton2;
	private GameObject arenaCoopButton1;
	private GameObject arenaCoopButton2;
	private GameObject arenaVSButton;

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

		defaultStoryArenaButtons = gameObject.transform.GetChild(1).gameObject;
		doublePlayersStoryArenaButtons = gameObject.transform.GetChild(2).gameObject;
		singleButton = gameObject.transform.GetChild(4).gameObject;
		doubleButton = gameObject.transform.GetChild(5).gameObject;
		p1ControlsButton = gameObject.transform.GetChild(7).gameObject;
		p2ControlsButton = gameObject.transform.GetChild(9).gameObject;
		playButton = gameObject.transform.GetChild(12).gameObject;

		storyButton1 = defaultStoryArenaButtons.transform.GetChild(0).gameObject;
		arenaCoopButton1 = defaultStoryArenaButtons.transform.GetChild(1).gameObject;

		storyButton2 = doublePlayersStoryArenaButtons.transform.GetChild(0).gameObject;
		arenaCoopButton2 = doublePlayersStoryArenaButtons.transform.GetChild(1).gameObject;
		arenaVSButton = doublePlayersStoryArenaButtons.transform.GetChild(2).gameObject;

		PlayMenuUpdate();
	}
	
	public void PlayMenuUpdate()
	{
		if (GameManager.singleGame)
		{
			singleButton.GetComponent<SpriteRenderer>().color = selectedColor;
			doubleButton.GetComponent<SpriteRenderer>().color = deselectedColor;

			defaultStoryArenaButtons.SetActive(true);
			doublePlayersStoryArenaButtons.SetActive(false);
			TogglesValues.coop = true;
		}
		else
		{
			singleButton.GetComponent<SpriteRenderer>().color = deselectedColor;
			doubleButton.GetComponent<SpriteRenderer>().color = selectedColor;

			defaultStoryArenaButtons.SetActive(false);
			doublePlayersStoryArenaButtons.SetActive(true);
		}

		if (TogglesValues.story)
		{
			storyButton1.GetComponent<SpriteRenderer>().color = selectedColor;
			storyButton2.GetComponent<SpriteRenderer>().color = selectedColor;

			arenaCoopButton1.GetComponent<SpriteRenderer>().color = deselectedColor;
			arenaCoopButton2.GetComponent<SpriteRenderer>().color = deselectedColor;

			arenaVSButton.GetComponent<SpriteRenderer>().color = deselectedColor;
		}
		else
		{
			storyButton1.GetComponent<SpriteRenderer>().color = deselectedColor;
			storyButton2.GetComponent<SpriteRenderer>().color = deselectedColor;
			
			if (TogglesValues.coop)
			{
				arenaCoopButton1.GetComponent<SpriteRenderer>().color = selectedColor;
				arenaCoopButton2.GetComponent<SpriteRenderer>().color = selectedColor;

				arenaVSButton.GetComponent<SpriteRenderer>().color = deselectedColor;
			}
			else
			{
				arenaCoopButton1.GetComponent<SpriteRenderer>().color = deselectedColor;
				arenaCoopButton2.GetComponent<SpriteRenderer>().color = deselectedColor;

				arenaVSButton.GetComponent<SpriteRenderer>().color = selectedColor;
			}
		}
	}

	public void SelectStory()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		TogglesValues.story = true;
		TogglesValues.coop = false;
		PlayMenuUpdate();
	}

	public void SelectArenaCoop()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		TogglesValues.story = false;
		TogglesValues.coop = true;
		PlayMenuUpdate();
	}

	public void SelectArenaVS()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		TogglesValues.story = false;
		TogglesValues.coop = false;
		PlayMenuUpdate();
	}

	public void SelectSingle()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		GameManager.singleGame = true;
		PlayMenuUpdate();
	}

	public void SelectDouble()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		GameManager.singleGame = false;
		PlayMenuUpdate();
	}

	public void SelectP1Controls()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		if (p1ControlsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == "Keyboard")
		{
			p1ControlsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Controller";
			p1ControlsButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Controller";
		}
		else
		{
			p1ControlsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Keyboard";
			p1ControlsButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keyboard";
		}
	}

	public void SelectP2Controls()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		if (p2ControlsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text == "Keyboard")
		{
			p2ControlsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Controller";
			p2ControlsButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Controller";
		}
		else
		{
			p2ControlsButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Keyboard";
			p2ControlsButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keyboard";
		}
	}

	public void Play()
	{
		if (aud != null && TogglesValues.sound)
		{
			aud.PlayOneShot(clickSound);
		}

		if (TogglesValues.story)
		{
			SceneManager.LoadScene("Prologue");
		}
		else
		{
			Instantiate(singlePlayerSetup);
			SceneManager.LoadScene("Arena");
		}
	}
}
