using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnNoFocus : MonoBehaviour
{
	public GameObject pauseObject;

	void OnApplicationPause()
	{
		Time.timeScale = 0f;
		pauseObject.SetActive(true);
	}

	void OnApplicationFocus()
	{
		Time.timeScale = 0f;
		pauseObject.SetActive(true);
	}
}
