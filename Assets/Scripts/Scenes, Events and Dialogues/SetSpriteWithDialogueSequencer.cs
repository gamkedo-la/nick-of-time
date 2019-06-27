using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteWithDialogueSequencer : MonoBehaviour {
	
	public GameObject objectToSet;
	
	public GameObject dialogueBoxObject;
	
	public Sprite[] sprites;
	
	public int dialogueNoOffset = 0;
	public int lastDialogueNo = 0;
	
	public bool selfAnimatorControl = true;
	public bool objectAnimatorControl = false;
	
	private Animator selfAnimator;
	private Animator objectAnimator;
	private SpriteRenderer sprRenderer;

	void Start () {
		if(selfAnimatorControl)
		{
			selfAnimator = GetComponent<Animator>();
			selfAnimator.enabled = false;
		}
		
		if(objectAnimatorControl)
		{
			objectAnimator = objectToSet.GetComponent<Animator>();
			objectAnimator.enabled = false;
		}
			
		if(objectToSet != null)
			sprRenderer = objectToSet.GetComponent<SpriteRenderer>();
	}
	
	void Update ()
	{
		int dialogueNo = dialogueBoxObject.GetComponent<DialogueBoxSwitch>().dialogueSequence.dialogueNo;
		
		if(dialogueNo >= dialogueNoOffset && objectToSet != null & sprites.Length > 0)
		{
			sprRenderer.sprite = sprites[dialogueNo - dialogueNoOffset];
		}
		
		if(dialogueNo >= lastDialogueNo)
		{
			if(selfAnimatorControl)
				selfAnimator.enabled = true;
			
			if(objectAnimatorControl)
				objectAnimator.enabled = true;
			
			enabled = false;
		}
	}
}
