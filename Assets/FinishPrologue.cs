using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPrologue : MonoBehaviour
{
	public PrologueCam prologueCam;

	public float delay = 0.9f;

	public void Update()
	{
		if (delay <= 0f)
			prologueCam.finishPrologue();
		else
			delay -= Time.deltaTime;
	}
}
