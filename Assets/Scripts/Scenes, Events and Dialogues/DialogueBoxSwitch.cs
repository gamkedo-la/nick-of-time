using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBoxSwitch : MonoBehaviour
{
	public GameObject inventoryPl1;
	public GameObject inventoryPl2;
	public GameObject minimap;

	public string proceedInput = "Submit";

	private GameObject pl1;
	private GameObject pl2;

	public AudioClip proceedSound;

	private AudioSource aud;

	[HideInInspector] public TextMeshProUGUI dialogueText = null;
	[HideInInspector] public DialogueSequence dialogueSequence;
	private SpriteRenderer sprRenderer;
	private GameObject nickFaces;
	private GameObject oldManFaces;
	private GameObject guardFaces;

	void Start()
	{
		pl1 = GameObject.Find("Player1");
		pl2 = GameObject.Find("Player2");

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

		sprRenderer = GetComponent<SpriteRenderer>();
		nickFaces = transform.GetChild(2).gameObject;
		oldManFaces = transform.GetChild(3).gameObject;
		guardFaces = transform.GetChild(4).gameObject;
	}

	void Update()
	{
		if (GameManager.singleGame)
		{
			dialogueText = transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
			dialogueSequence = transform.GetChild(0).gameObject.GetComponent<DialogueSequence>();
		}
		else
		{
			dialogueText = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
			dialogueSequence = transform.GetChild(1).gameObject.GetComponent<DialogueSequence>();
		}

		if (dialogueText.text == " " || dialogueText.text == "")
		{
			if (sprRenderer.enabled)
			{
				sprRenderer.enabled = false;

				if (pl1)
					pl1.GetComponent<PlayerController>().enabled = true;

				if (inventoryPl1)
					inventoryPl1.SetActive(true);
					
				if (pl2)
					pl2.GetComponent<PlayerController>().enabled = true;

				if (inventoryPl2)
					inventoryPl2.SetActive(true);

				if (minimap)
					minimap.SetActive(true);

				Subtitles.Enable();
			}
		}
		else
		{
			sprRenderer.enabled = true;

            if (pl1)
                pl1.GetComponent<PlayerController>().enabled = false;

			if (inventoryPl1)
				inventoryPl1.SetActive(false);
				
			if (pl2)
                pl2.GetComponent<PlayerController>().enabled = false;

			if (inventoryPl2)
				inventoryPl2.SetActive(false);

			if (minimap)
				minimap.SetActive(false);

			Subtitles.Disable();

			if(dialogueSequence.dialogueNo >= 0)
				checkForDialogueStringTags(dialogueSequence.dialogues[dialogueSequence.dialogueNo]);
		}

		if ((Input.GetButtonDown(proceedInput))
			&& sprRenderer.enabled)
		{
			if (aud != null && TogglesValues.sound)
				aud.PlayOneShot(proceedSound);

			dialogueSequence.dialogueNo++;

			checkForDialogueStringTags(dialogueSequence.dialogues[dialogueSequence.dialogueNo]);
		}
	}

	public void checkForDialogueStringTags(string text)
	{
		//Player 1 text color is #ff6666
		//Player 2 text color is #66ff66
		//Nick text color is #ffbb00 - #ffbb06
		//Other NPCs color is #ffff00 - #ffff05



		if (text.Contains("#ffff00>Old Man")) //Old Man Smile
		{
			oldManFaces.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff01>Old Man")) //Old Man Happy
		{
			oldManFaces.transform.GetChild(1).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff02>Old Man")) //Old Man Sad
		{
			oldManFaces.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff03>Old Man")) //Old Man Shock
		{
			oldManFaces.transform.GetChild(3).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff04>Old Man")) //Old Man Evil
		{
			oldManFaces.transform.GetChild(4).gameObject.SetActive(true);
		}
		else
		{
			for (int i = 0; i < 5; i++)
			{
				oldManFaces.transform.GetChild(i).gameObject.SetActive(false);
			}
		}



		if (text.Contains("#ffff00>Guard")) //Guard Smile
		{
			guardFaces.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff01>Guard")) //Guard Happy
		{
			guardFaces.transform.GetChild(1).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff02>Guard")) //Guard Sad
		{
			guardFaces.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff03>Guard")) //Guard Shock
		{
			guardFaces.transform.GetChild(3).gameObject.SetActive(true);
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				guardFaces.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
		


		if (text.Contains("#ffbb00>Nick")) //Nick Smile
		{
			nickFaces.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb01>Nick")) //Nick Happy
		{
			nickFaces.transform.GetChild(1).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb02>Nick")) //Nick Sad
		{
			nickFaces.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb03>Nick")) //Nick Depressed
		{
			nickFaces.transform.GetChild(3).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb04>Nick")) //Nick Serious
		{
			nickFaces.transform.GetChild(4).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb05>Nick")) //Nick Shock
		{
			nickFaces.transform.GetChild(5).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb06>Nick")) //Nick Evil
		{
			nickFaces.transform.GetChild(6).gameObject.SetActive(true);
		}
		else
		{
			for (int i = 0; i < 7; i++)
			{
				nickFaces.transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}
