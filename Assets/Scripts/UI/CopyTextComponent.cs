using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CopyTextComponent : MonoBehaviour {
	
	public GameObject textObject;
	
	public bool onlyTextString = true;
	
	private TextMeshPro textComponent;
	private TextMeshPro otherTextComponent;

	void Start () {
		textComponent = GetComponent<TextMeshPro>();
		otherTextComponent = textObject.GetComponent<TextMeshPro>();
	}
	
	void Update () {
		if(onlyTextString)
		{
			textComponent.text = otherTextComponent.text;
		}
		else
		{
			textComponent = otherTextComponent;
		}
	}
}
