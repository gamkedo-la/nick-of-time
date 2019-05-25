using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStartScript : MonoBehaviour {
	
	public float delay = 1f;
	
	public GameObject pl1;
	public GameObject pl2;
	public GameObject dialogueBoxObject;

	void Start () {
		if(pl1)
		{
			pl1.GetComponent<PlayerController>().enabled = false;
		}
		if(pl2)
		{
			pl2.GetComponent<PlayerController>().enabled = false;
		}
	}
	
	void Update () {
		if(delay <= 0f)
		{
			dialogueBoxObject.GetComponent<DialogueBoxSwitch>().dialogueSequence.dialogueNo++;
			pl1.GetComponent<PlayerController>().enabled = true;
			pl2.GetComponent<PlayerController>().enabled = true;
			Destroy(gameObject);
		}
		
		delay -= Time.deltaTime;
	}
}
