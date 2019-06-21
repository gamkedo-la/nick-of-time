using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBoxSwitch : MonoBehaviour
{
	public GameObject pl1;
	public GameObject pl2;
	public GameObject inventoryPl1;
	public GameObject inventoryPl2;
    private bool disabledP1 = false;
    private bool disabledP2 = false;

	public string proceedInput = "Submit";
	public string proceedInput2= "Jump"; // to allow SPACE or ENTER to proceed

	public AudioClip proceedSound;

	private AudioSource aud;

	[HideInInspector] public TextMeshProUGUI dialogueText = null;
	[HideInInspector] public DialogueSequence dialogueSequence;
	private SpriteRenderer sprRenderer;
	private GameObject nickFaces;
	private GameObject oldManFaces;
	private GameObject slenderGuardFaces;
	private GameObject fatGuardFaces;

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

		sprRenderer = GetComponent<SpriteRenderer>();
		nickFaces = transform.GetChild(2).gameObject;
		oldManFaces = transform.GetChild(3).gameObject;
		slenderGuardFaces = transform.GetChild(4).gameObject;
		fatGuardFaces = transform.GetChild(5).gameObject;
	}

	void Update()
	{

		if (dialogueText.text == " " || dialogueText.text == "")
		{
			sprRenderer.enabled = false;

            if (pl1 && disabledP1)
            {
                disabledP1 = false;
                pl1.GetComponent<PlayerController>().enabled = true;

				if (inventoryPl1) inventoryPl1.SetActive(true);
            }
            if (pl2 && disabledP2)
            {
                disabledP2 = false;
                pl2.GetComponent<PlayerController>().enabled = true;

				if (inventoryPl2) inventoryPl2.SetActive(true);
			}
		}
		else if (!sprRenderer.enabled)
		{
			sprRenderer.enabled = true;

            if (pl1)
            {
                disabledP1 = true;
                pl1.GetComponent<PlayerController>().enabled = false;

				if (inventoryPl1) inventoryPl1.SetActive(false);
			}
            if (pl2)
            {
                disabledP2 = true;
                pl2.GetComponent<PlayerController>().enabled = false;

				if (inventoryPl2) inventoryPl2.SetActive(false);
			}

			checkForDialogueStringTags(dialogueSequence.dialogues[dialogueSequence.dialogueNo]);
		}

		if ((Input.GetButtonDown(proceedInput) || Input.GetButtonDown(proceedInput2))
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
		//0 for Dagger
		if (text.Contains("<color=#00ff00>Dagger"))
		{
			if (pl1) pl1.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<WeaponPossession>().weaponID = 0;
			if (pl2) pl2.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<WeaponPossession>().weaponID = 0;
		}
		//1 for Katana
		else if (text.Contains("<color=#00ff00>Katana"))
		{
			if (pl1) pl1.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<WeaponPossession>().weaponID = 0;
			if (pl2) pl2.transform.GetChild(3).GetChild(0).GetChild(0).gameObject.GetComponent<WeaponPossession>().weaponID = 0;
		}

		//Player 1 text color is #ff6666
		//Player 2 text color is #6666ff
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



		if (text.Contains("#ffff00>Slender Guard")) //Slender Guard Smile
		{
			slenderGuardFaces.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff01>Slender Guard")) //Slender Guard Happy
		{
			slenderGuardFaces.transform.GetChild(1).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff02>Slender Guard")) //Slender Guard Sad
		{
			slenderGuardFaces.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff03>Slender Guard")) //Slender Guard Shock
		{
			slenderGuardFaces.transform.GetChild(3).gameObject.SetActive(true);
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				slenderGuardFaces.transform.GetChild(i).gameObject.SetActive(false);
			}
		}



		if (text.Contains("#ffff00>Fat Guard")) //Fat Guard Smile
		{
			fatGuardFaces.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff01>Fat Guard")) //Fat Guard Happy
		{
			fatGuardFaces.transform.GetChild(1).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff02>Fat Guard")) //Fat Guard Sad
		{
			fatGuardFaces.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffff03>Fat Guard")) //Fat Guard Shock
		{
			fatGuardFaces.transform.GetChild(3).gameObject.SetActive(true);
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				fatGuardFaces.transform.GetChild(i).gameObject.SetActive(false);
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
