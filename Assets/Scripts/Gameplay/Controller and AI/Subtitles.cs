using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// descriptive text for game events, one per player
// example: "picked up a potion"
public class Subtitles : MonoBehaviour
{
    public float fadePerFrame = 0.005f;
	
	private TextMeshProUGUI[] texts;
    private float alpha = 1f;

	static public void AddPlayer1Subtitle(string str)
	{
		if (GameManager.singleGame)
			singlePlayerSubtitles.Caption(str);
		else
		{
			p1Subtitles[0].Caption(str);
			p1Subtitles[1].Caption(str);
		}
	}

	static public void AddPlayer2Subtitle(string str)
	{
		p2Subtitles[0].Caption(str);
		p2Subtitles[1].Caption(str);
	}

	static private Subtitles singlePlayerSubtitles;
	static private Subtitles[] p1Subtitles = new Subtitles[2];
	static private Subtitles[] p2Subtitles = new Subtitles[2];
	
	void Start()
    {
		texts = new TextMeshProUGUI[transform.childCount];

		for (int i = 0; i < transform.childCount; i++)
		{
			texts[i] = transform.GetChild(i).gameObject.GetComponent<TextMeshProUGUI>();
		}

		if (name.Contains("P1"))
		{
			if (p1Subtitles[0] == null) p1Subtitles[0] = this;
			else if (p1Subtitles[1] == null) p1Subtitles[1] = this;
		}
		else if (name.Contains("P2"))
		{
			if (p2Subtitles[0] == null) p2Subtitles[0] = this;
			else if (p2Subtitles[1] == null) p2Subtitles[1] = this;
		}

		if (name == "P1JointSubtitles")
		{
			singlePlayerSubtitles = this;

			if (GameManager.singleGame)
				singlePlayerSubtitles.gameObject.transform.localPosition = new Vector3(0f,
					singlePlayerSubtitles.gameObject.transform.localPosition.y, singlePlayerSubtitles.gameObject.transform.localPosition.z);
		}

		if (!TogglesValues.subtitles)
		{
			enabled = false;

			for (int n = 0; n < transform.childCount; n++)
				texts[n].enabled = false;
		}
    }
	
    void Update()
    {
		for (int i = 0; i < transform.childCount; i++)
		{
			Color col = texts[i].color;
			col.a = Mathf.Lerp(col.a, 0f, fadePerFrame);
			texts[i].color = col;
		}
    }

    public void Caption(string str)
	{
		for (int n = 0; n < transform.childCount; n++)
		{
			texts[n].color -= new Color(0f, 0f, 0f, 0.1f);
			texts[n].gameObject.transform.localPosition -= new Vector3(0f, 40f, 0f);
		}
		
		for (int i = 0; i < transform.childCount; i++)
		{
			if (texts[i].gameObject.transform.localPosition == new Vector3(0f, -80f, 0f))
			{
				texts[i].text = str;
				texts[i].color = Color.white;
				texts[i].gameObject.transform.localPosition = new Vector3(0f, 40f, 0f);
				break;
			}
		}
    }
}
