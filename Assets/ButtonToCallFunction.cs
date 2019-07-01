using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonToCallFunction : MonoBehaviour
{
	public UnityEvent onClick;
	
	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0))
		{
			onClick.Invoke();
		}
	}
}
