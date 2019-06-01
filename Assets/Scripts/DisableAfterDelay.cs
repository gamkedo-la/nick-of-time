using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
	public float delay = 1f;    

    void Update()
    {
		if (delay <= 0f)
		{
			Destroy(this);
			gameObject.SetActive(false);
		}
		else
		{
			delay -= Time.deltaTime;
		}
    }
}
