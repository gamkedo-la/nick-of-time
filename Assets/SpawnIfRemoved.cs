using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIfRemoved : MonoBehaviour
{
	private GameObject inactiveFirstChildObject;

    void Start()
    {
		inactiveFirstChildObject = transform.GetChild(0).gameObject;
    }
	
    void Update()
    {
		if (transform.childCount < 2)
		{
			GameObject obj = Instantiate(inactiveFirstChildObject, transform.position, inactiveFirstChildObject.transform.rotation);
			obj.SetActive(true);
			obj.transform.parent = gameObject.transform;
		}
    }
}
