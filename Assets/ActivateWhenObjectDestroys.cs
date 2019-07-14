using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateWhenObjectDestroys : MonoBehaviour
{
	public GameObject objectThatWillDestroy;
	public GameObject objectToActivate;
	
    void Update()
    {
		if (objectThatWillDestroy == null)
			objectToActivate.SetActive(true);
    }
}
