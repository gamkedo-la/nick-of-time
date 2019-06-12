using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBoxSwitch : MonoBehaviour
{

	public GameObject pl1;
	public GameObject pl2;
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
            }
            if (pl2 && disabledP2)
            {
                disabledP2 = false;
                pl2.GetComponent<PlayerController>().enabled = true;
            }
		}
		else if (!sprRenderer.enabled)
		{
			sprRenderer.enabled = true;

            if (pl1)
            {
                disabledP1 = true;
                pl1.GetComponent<PlayerController>().enabled = false;
            }
            if (pl2)
            {
                disabledP2 = true;
                pl2.GetComponent<PlayerController>().enabled = false;
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

		if (text.Contains("#ffbb00")) //Smile
		{
			nickFaces.transform.GetChild(0).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb01")) //Happy
		{
			nickFaces.transform.GetChild(1).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb02")) //Sad
		{
			nickFaces.transform.GetChild(2).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb03")) //Depressed
		{
			nickFaces.transform.GetChild(3).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb04")) //Serious
		{
			nickFaces.transform.GetChild(4).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb05")) //Shock
		{
			nickFaces.transform.GetChild(5).gameObject.SetActive(true);
		}
		else if (text.Contains("#ffbb06")) //Evil
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
