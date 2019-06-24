using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal: MonoBehaviour
{
	public float waitForSeconds = 1f;
	public float revealInSeconds = 3f;

	private TextMeshPro text;
	private float revealTimer = 0f;
	
    void Start()
    {
		text = gameObject.GetComponent<TextMeshPro>();
		text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
		revealTimer = revealInSeconds;
    }
	
    void Update()
    {
		if (waitForSeconds <= 0f)
		{
			Color col = text.color;
			col.a = 1f - (revealTimer / revealInSeconds);
			text.color = col;

			revealTimer -= Time.deltaTime;
		}
		else
			waitForSeconds -= Time.deltaTime;
    }
}
