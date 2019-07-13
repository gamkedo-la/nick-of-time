using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperViewScript : MonoBehaviour
{
	private void OnEnable()
	{
		if (ActivateSingleMode.currentInstance != null)
		{
			ActivateSingleMode.currentInstance.paused = true;
			ActivateSingleMode.currentInstance.duelCamToSingleCam();
		}
		Time.timeScale = 0f;
	}

	void Update()
    {
	}
}
