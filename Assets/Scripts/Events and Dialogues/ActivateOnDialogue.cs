using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnDialogue : MonoBehaviour {
	
	public GameObject dialogueBoxObject;
	
	public int dialogueNo;
	
	public GameObject objectToActivate;
	
	public bool selfDeactivate = true;
	
	void Start()
	{
	}
	
	void Update()
	{
		if(dialogueBoxObject.GetComponent<DialogueBoxSwitch>().dialogueSequence.dialogueNo == dialogueNo)
		{
			objectToActivate.SetActive(true);
			
			if(selfDeactivate)
				enabled = false;
		}
	}
}
