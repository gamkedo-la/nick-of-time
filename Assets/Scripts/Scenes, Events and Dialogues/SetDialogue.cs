using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDialogue : MonoBehaviour {
	
	public GameObject dialogueObject;

	public bool increment = false;
	public bool doSinglePlayerSwitch = false;
	
	void Start () {
		if((increment && !doSinglePlayerSwitch)
			|| (increment && doSinglePlayerSwitch && GameManager.singleGame))
		{
			incrementDialogueNo();
			
			increment = false;
		}
	}
	
	void Update () {
		if((increment && !doSinglePlayerSwitch)
			|| (increment && doSinglePlayerSwitch && GameManager.singleGame))
		{
			incrementDialogueNo();
			
			increment = false;
		}
	}
	
	public void setDialogueNo(int no)
	{
		if(!doSinglePlayerSwitch || (doSinglePlayerSwitch && GameManager.singleGame))
			dialogueObject.GetComponent<DialogueSequence>().setDialogueNo(no);
	}
	
	public void incrementDialogueNo()
	{
		if(!doSinglePlayerSwitch || (doSinglePlayerSwitch && GameManager.singleGame))
			dialogueObject.GetComponent<DialogueSequence>().incrementDialogueNo();
	}
	
	public void setNextDialogue()
	{
		if(!doSinglePlayerSwitch || (doSinglePlayerSwitch && GameManager.singleGame))
			dialogueObject.GetComponent<DialogueSequence>().setNextDialogue();
	}
}
