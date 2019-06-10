using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPieceObject : MonoBehaviour
{
	public enum TutorialPieceType
	{
		Movement,
		Attack,
		Dash,
		Push,
		Throw,
		Inventory
	}

	public TutorialPieceType type;
	public float waitForSeconds = 3f;
	public float revealInSeconds = 1f;

	private TextMeshPro tutText;
	private float revealTimer = 0f;
	
    void Start()
    {
		tutText = gameObject.GetComponent<TextMeshPro>();
		tutText.color = new Color(tutText.color.r, tutText.color.g, tutText.color.b, 0f);
		revealTimer = revealInSeconds;
    }
	
    void Update()
    {
		if (waitForSeconds <= 0f)
		{
			Color col = tutText.color;
			col.a = 1f - (revealTimer / revealInSeconds);
			tutText.color = col;

			revealTimer -= Time.deltaTime;
		}
		else
			waitForSeconds -= Time.deltaTime;
    }
}
