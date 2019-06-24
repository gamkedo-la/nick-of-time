using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CopyTextComponent : MonoBehaviour {
	
	public GameObject textObject;
	
	public bool onlyTextString = true;
	
	private TextMeshPro textComponent;
	private TextMeshPro otherTextComponent;

	private TextMeshProUGUI textComponentUI;
	private TextMeshProUGUI otherTextComponentUI;

	void Start () {
		textComponent = GetComponent<TextMeshPro>();
		otherTextComponent = textObject.GetComponent<TextMeshPro>();

		textComponentUI = GetComponent<TextMeshProUGUI>();
		otherTextComponentUI = GetComponent<TextMeshProUGUI>();

	}
	
	void Update () {
		if(onlyTextString)
		{
			if (textComponent != null)
			{
				if (otherTextComponent != null)
					textComponent.text = otherTextComponent.text;
				else
					textComponent.text = otherTextComponentUI.text;
			}
			else
			{
				if (otherTextComponent != null)
					textComponentUI.text = otherTextComponent.text;
				else
					textComponentUI.text = otherTextComponentUI.text;
			}
		}
		else
		{
			if(textComponent != null)
				textComponent = otherTextComponent;
			else
				textComponentUI = otherTextComponentUI;
		}
	}
}
