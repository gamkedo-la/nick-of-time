using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnNoFocus : MonoBehaviour
{
	public GameObject pauseObject;

	void OnApplicationPause()
	{
		if (ActivateSingleMode.currentInstance != null)
		{
			ActivateSingleMode.currentInstance.paused = true;
			ActivateSingleMode.currentInstance.duelCamToSingleCam();
		}
		pauseObject.SetActive(true);
		Time.timeScale = 0f;
	}

	void OnApplicationFocus()
	{
		if (ActivateSingleMode.currentInstance != null)
		{
			ActivateSingleMode.currentInstance.paused = true;
			ActivateSingleMode.currentInstance.duelCamToSingleCam();
		}
		pauseObject.SetActive(true);
		Time.timeScale = 0f;
	}
}
