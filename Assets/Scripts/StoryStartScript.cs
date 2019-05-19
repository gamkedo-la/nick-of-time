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
			pl1.GetComponent<Animator>().SetBool("layDown", true);
			pl1.GetComponent<PlayerActions>().enabled = false;
		}
		if(pl2)
		{
			pl2.GetComponent<Animator>().SetBool("layDown", true);
			pl2.GetComponent<PlayerActions>().enabled = false;
		}
	}
	
	void Update () {
		if(delay <= 0f)
		{
			if(pl1)
			{
				pl1.GetComponent<Animator>().SetBool("layDown", false);
				pl1.GetComponent<PlayerActions>().enabled = true;
			}
			if(pl2)
			{
				pl2.GetComponent<Animator>().SetBool("layDown", false);
				pl2.GetComponent<PlayerActions>().enabled = true;
			}
			
			dialogueBoxObject.GetComponent<DialogueBoxSwitch>().dialogueSequence.dialogueNo++;
			
			Destroy(gameObject);
		}
		
		delay -= Time.deltaTime;
	}
}
