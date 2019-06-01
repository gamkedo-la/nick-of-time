using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnSamePosition : MonoBehaviour
{
	public GameObject[] objectsToDisable;
	public bool enableIfDifferentPosition = true;

    void Start()
    {
        
    }


    void Update()
    {
		foreach (var o in objectsToDisable)
		{
			if (o.transform.position == transform.position)
			{
				o.SetActive(false);
			}
			else if(enableIfDifferentPosition)
			{
				o.SetActive(true);
			}
		}
    }
}
