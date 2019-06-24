using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryStartScript : MonoBehaviour {
	
	public float delay = 1f;
	
	public GameObject pl1;
	public GameObject pl2;
	public GameObject dialogueBoxObject;

	void Start () {
		
	}
	
	void Update () {
		if (delay <= 0f)
		{
			dialogueBoxObject.GetComponent<DialogueBoxSwitch>().dialogueSequence.dialogueNo++;
			if (pl1)
			{
				pl1.GetComponent<PlayerController>().enabled = true;
			}
			if (pl2)
			{
				pl2.GetComponent<PlayerController>().enabled = true;
			}
			Destroy(gameObject);
		}
		else
		{
			if (pl1)
			{
				pl1.GetComponent<PlayerController>().enabled = false;
			}
			if (pl2)
			{
				pl2.GetComponent<PlayerController>().enabled = false;
			}
		}
		
		delay -= Time.deltaTime;
	}
}
