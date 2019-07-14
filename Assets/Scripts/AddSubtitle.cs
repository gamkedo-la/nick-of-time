using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSubtitle : MonoBehaviour
{
	public string subtitleTextToAdd = "";

	public bool toPlayer1 = false;
	public bool toPlayer2 = false;

	public bool deleteObject = false;
	public bool deleteComponent = true;
	
    void Start()
    {
		if (toPlayer1) Subtitles.AddPlayer1Subtitle(subtitleTextToAdd);
		if (toPlayer2 && !GameManager.singleGame) Subtitles.AddPlayer2Subtitle(subtitleTextToAdd);

		if (deleteObject) Destroy(gameObject);
		if (deleteComponent) Destroy(this);
    }
}
