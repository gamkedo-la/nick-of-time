using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueSequence : MonoBehaviour {
	
	public bool nextDialogue = false;
	public int dialogueNo = 0;

	[TextArea(1, 2)]
	public string[] dialogues;
	
	public float delay = 0f;
	private float timer = 0f;
	
	private TextMeshPro text;
	private TextMeshProUGUI textCanvas;
	
	private int prevDialogueNo = -1;

	void Start () {
		text = GetComponent<TextMeshPro>();

		if (text == null)
			textCanvas = GetComponent<TextMeshProUGUI>();
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
