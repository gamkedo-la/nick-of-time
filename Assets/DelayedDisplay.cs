using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DelayedDisplay : MonoBehaviour
{
	public bool sprite = true;
	public bool text = false;
	public bool text_noUI = false;

	public float delay = 0.15f;

    void Start()
    {
		if(sprite)
			GetComponent<SpriteRenderer>().enabled = false;
		if (text)
			GetComponent<TextMeshProUGUI>().enabled = false;
		if (text_noUI)
			GetComponent<TextMeshPro>().enabled = false;
	}
	
    void Update()
    {
		if (delay <= 0f)
		{
			if (sprite)
				GetComponent<SpriteRenderer>().enabled = true;
			if (text)
				GetComponent<TextMeshProUGUI>().enabled = true;
			if (text_noUI)
				GetComponent<TextMeshPro>().enabled = true;
			Destroy(this);
		}
		else
			delay -= Time.deltaTime;
    }
}
