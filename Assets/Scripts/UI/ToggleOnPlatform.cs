using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleOnPlatform : MonoBehaviour
{
	public GameObject[] objectsToEnable;
	public GameObject[] objectsToDisable;

    void Start()
    {
		//#if UNITY_EDITOR
#if UNITY_WEBGL //#elif UNITY_WEBGL

		foreach (var o in objectsToEnable)
		{
			o.SetActive(true);
		}

		foreach (var o in objectsToDisable)
		{
			o.SetActive(false);
		}

#endif
	}
}
