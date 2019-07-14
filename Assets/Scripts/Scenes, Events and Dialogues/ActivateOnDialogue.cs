using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateOnDialogue : MonoBehaviour {
	
	public GameObject dialogueBoxObject;
	
	public int dialogueNo;
	
	public GameObject objectToActivate;
	
	public bool selfDeactivate = true;
	public bool animator = false;
	public bool collider = false;
	
	void Start()
	{
	}
	
	void Update()
	{
		if(dialogueBoxObject.GetComponent<DialogueBoxSwitch>().dialogueSequence.dialogueNo == dialogueNo)
		{
			objectToActivate.SetActive(true);
			if (animator) objectToActivate.GetComponent<Animator>().enabled = true;
			if (collider) objectToActivate.GetComponent<Collider2D>().enabled = true;

			if (selfDeactivate)
				enabled = false;
		}
	}
}
