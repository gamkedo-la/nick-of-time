using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueSequence : MonoBehaviour
{
	public string file;

	[HideInInspector] public bool nextDialogue = false;
	[HideInInspector] public int dialogueNo = 0;

	[HideInInspector] public string[] dialogues;

	public float delay = 0f;

	private float timer = 0f;
	
	private TextMeshPro text;
	private TextMeshProUGUI textCanvas;
	
	private int prevDialogueNo = -1;

	private int totalDialogues = -1;
	
	private StreamReader reader = null;
	private string line = " "; // assigned to allow first line to be read below
	
	void Start () {
		text = GetComponent<TextMeshPro>();

		if (text == null)
			textCanvas = GetComponent<TextMeshProUGUI>();

		reader = new StreamReader(file);

		int dialogueIndex = 0;
		while (line != null)
		{
			line = reader.ReadLine();
			if (totalDialogues <= -1)
			{
				totalDialogues = System.Convert.ToInt32(line);
				dialogues = new string[totalDialogues];
			}
			else if(line != null)
			{
				dialogues[dialogueIndex++] = line;
			}
		}

		reader.Dispose();
		reader.Close();
	}
	
	void Update () {
		if(timer <= 0f)
		{
			if(nextDialogue)
			{
				dialogueNo++;
				nextDialogue = false;
				timer = delay;
			}
		}
		
		if(dialogueNo != prevDialogueNo)
		{
			if (text != null)
				text.text = dialogues[dialogueNo];
			else
				textCanvas.text = dialogues[dialogueNo];
		}
		
		prevDialogueNo = dialogueNo;
		
		timer -= Time.deltaTime;
	}
	
	
	public void setDialogueNo(int no)
	{
		dialogueNo = no;
	}
	
	public void incrementDialogueNo()
	{
		dialogueNo++;
	}
	
	public void setNextDialogue()
	{
		nextDialogue = true;
	}
}
