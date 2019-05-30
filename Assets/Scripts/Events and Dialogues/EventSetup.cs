using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSetup : MonoBehaviour {
	
	public GameObject fadeInObject;
	
	public GameObject pl1;
	public GameObject pl2;
	
	public bool hidePlayers = false;
	
	public Transform pl1StartPosition;
	public Transform pl2StartPosition;
	
	public Transform[] pl1AfterPositions;
	public Transform[] pl2AfterPositions;
	
	public float lerpPercent = 0.05f;
	public float lerpMinDistance = 0.2f;
	
	public GameObject dialogueBoxObject;
	public int lastDialogueNo;
	
	public float delay = 0f;
	
	public bool selfAnimatorControl = false;
	
	private bool pl1Done = false;
	private bool pl2Done = false;
	
	private SetDialogue[] setDialogueComponents;
	
	private int pl1MoveTo = 0;
	private int pl2MoveTo = 0;
	
	void Start () {
		setDialogueComponents = GetComponents<SetDialogue>();
		
		if(fadeInObject)
			Instantiate(fadeInObject);
		
		if(pl1)
			pl1.transform.position = pl1StartPosition.transform.position;
		
		if(pl2)
			pl2.transform.position = pl2StartPosition.transform.position;
		
		if(selfAnimatorControl)
			GetComponent<Animator>().enabled = false;
		
		if(hidePlayers)
		{
			if(pl1)
			{
				pl1.GetComponent<Animator>().enabled = false;
				pl1.GetComponent<SpriteRenderer>().enabled = false;

				for (int i = 0; i < pl1.transform.childCount; i++)
				{
					SpriteRenderer sprRend = pl1.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
					if(sprRend != null)
						sprRend.enabled = false;
				}
			}
			
			if(pl2)
			{
				pl2.GetComponent<Animator>().enabled = false;
				pl2.GetComponent<SpriteRenderer>().enabled = false;

				for (int i = 0; i < pl2.transform.childCount; i++)
				{
					SpriteRenderer sprRend = pl2.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>();
					if (sprRend != null)
						sprRend.enabled = false;
				}
			}
		}
	}
	
	void Update () {
		
		if(dialogueBoxObject != null && dialogueBoxObject.GetComponent<DialogueBoxSwitch>().dialogueSequence.dialogueNo <= lastDialogueNo)
		{
			
			if(pl1)
			{
				pl1.GetComponent<PlayerController>().enabled = false;
			
				if(pl1AfterPositions.GetLength(0) > 0 && pl1AfterPositions.GetLength(0) > pl1MoveTo)
				{
					pl1.transform.position = Vector3.MoveTowards( pl1.transform.position, pl1AfterPositions[pl1MoveTo].position, lerpPercent );
				
					if(Vector3.Distance(pl1.transform.position, pl1AfterPositions[pl1MoveTo].position) <= lerpMinDistance)
						pl1MoveTo++;
				
					pl1.GetComponent<Animator>().SetBool("isWalking", true);
				}
				else
				{
					if(selfAnimatorControl)
						GetComponent<Animator>().enabled = true;
				
					pl1.GetComponent<Animator>().SetBool("isWalking", false);
				
					if(!pl1Done)
						setDialogueComponents[0].increment = true;
				
					pl1Done = true;
				}
			}
		
			if(pl2)
			{
				pl2.GetComponent<PlayerController>().enabled = false;
			
				if(pl2AfterPositions.GetLength(0) > 0 && pl2AfterPositions.GetLength(0) > pl2MoveTo)
				{
					pl2.transform.position = Vector3.MoveTowards( pl2.transform.position, pl2AfterPositions[pl2MoveTo].position, lerpPercent );
				
					if(Vector3.Distance(pl2.transform.position, pl2AfterPositions[pl2MoveTo].position) <= lerpMinDistance)
						pl2MoveTo++;
				
					pl2.GetComponent<Animator>().SetBool("isWalking", true);
				}
				else
				{
					if(selfAnimatorControl)
						GetComponent<Animator>().enabled = true;
				
					pl2.GetComponent<Animator>().SetBool("isWalking", false);
				
					if(!pl2Done)
						setDialogueComponents[1].increment = true;
				
					pl2Done = true;
				}
			}
		
		}
		else if(delay <= 0f)
		{
			if(pl1) pl1.GetComponent<PlayerController>().enabled = true;
			if(pl2) pl2.GetComponent<PlayerController>().enabled = true;
			
			if(hidePlayers)
			{
				if(pl1)
				{
					pl1.GetComponent<Animator>().enabled = true;
					pl1.GetComponent<SpriteRenderer>().enabled = true;
			
					for(int i = 0; i < pl1.transform.childCount; i++)
						pl1.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = true;
				}
			
				if(pl2)
				{
					pl2.GetComponent<Animator>().enabled = true;
					pl2.GetComponent<SpriteRenderer>().enabled = true;
			
					for(int i = 0; i < pl2.transform.childCount; i++)
						pl2.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = true;
				}
			}
			
			enabled = false;
		}
		
		delay -= Time.deltaTime;
	}
	
	public void commandSetDialogueToIncrement()
	{
		if(GameManager.singleGame)
			setDialogueComponents[0].increment = true;
		else
			setDialogueComponents[1].increment = true;
	}
}
