using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToTransform : MonoBehaviour
{
	public float lerpPercent = 0.1f;
	public Transform tr;
	public Vector3 offset = new Vector3 (0f, 0f, -10.0f);
	
	public Transform plTr;
	public float playerFocusFactor = 0.5f;

	public bool onlyXAxis = false;
	
	public bool deathFocus = true;
	
	private Transform trPrev;

	void Start ()
	{
		
	}

	void Update ()
	{
		if(tr == null && trPrev == null && plTr == null)
		{
			enabled = false;
			return;
		}
		else if(tr == null && trPrev != null)
		{
			tr = trPrev;
		}
		
		Vector3 plPos = tr.position;

		if(onlyXAxis)
			plPos = new Vector3 ( plPos.x, transform.position.y, plPos.z);
		
		deathFocusTransform();

		transform.position = Vector3.Lerp ( transform.position, plPos + offset, lerpPercent );
		
		playerFocusTransform();
	}
	
	void deathFocusTransform()
	{
		if(plTr != null && deathFocus && plTr.gameObject.GetComponent<HitCheck>().hp <= 0f)
		{
			trPrev = tr;
			tr = plTr;
		}
	}
	
	void playerFocusTransform()
	{
		if(plTr != null && plTr != tr)
		{
			transform.position = Vector3.Lerp ( transform.position, plTr.position + offset, lerpPercent * playerFocusFactor );
		}
	}
}
