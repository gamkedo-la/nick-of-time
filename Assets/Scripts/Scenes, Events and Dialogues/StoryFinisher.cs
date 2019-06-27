using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryFinisher : MonoBehaviour
{
	private GameObject player1;
	private GameObject player2;

	public GameObject otherStoryFinisherObject;

	void Start()
    {
		player1 = GameObject.Find("Player1");
		player2 = GameObject.Find("Player2");
		LevelManager.triggerCount++;
    }
	
    void Update()
    {
        
    }

	public void FinishStory()
	{
		LevelManager.triggerCount--;

		player1.transform.position = new Vector3(transform.position.x + 0.16f, transform.position.y, player1.transform.position.z);
		player1.GetComponent<PlayerController>().enabled = false;

		if (player2 != null)
		{
			player2.transform.position = new Vector3(transform.position.x - 0.16f, transform.position.y, player1.transform.position.z);
			player2.GetComponent<PlayerController>().enabled = false;
		}

		if(otherStoryFinisherObject != null)
			otherStoryFinisherObject.GetComponent<StoryFinisher>().FinishStory();
	}
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
		{
			FinishStory();
		}
	}
}
