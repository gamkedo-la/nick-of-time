using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterDelay : MonoBehaviour
{
	public float delay = 1f;

	private void Start()
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			LightEmitterObject leo = gameObject.transform.GetChild(i).gameObject.GetComponent<LightEmitterObject>();
			if (leo != null)
				leo.disable = true;
		}
	}

	public void Revert()
	{
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			LightEmitterObject leo = gameObject.transform.GetChild(i).gameObject.GetComponent<LightEmitterObject>();
			if (leo != null)
				leo.disable = false;
		}

		Destroy(this);
	}

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
