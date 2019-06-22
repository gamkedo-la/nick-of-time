using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventItem : MonoBehaviour
{
	public AreaTriggerManager areaTriggerManager;
	public int triggerIndex = -1;

	[Space]
	public GameObject newEventObject;
	public GameObject oldEventObject;

	[Space]
	public GameObject[] activate;
	public GameObject[] deactivate;

	[Space]
	public bool destroy = true;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name.Contains("Player"))
		{
			foreach (var obj in areaTriggerManager.triggers[triggerIndex].objectToActivate)
			{
				if (obj.obj == oldEventObject)
				{
					obj.obj = newEventObject;
					areaTriggerManager.triggers[triggerIndex].isObjectsActivationDone = false;
					break;
				}
			}

			foreach (var obj in activate)
				obj.SetActive(true);

			foreach (var obj in deactivate)
				obj.SetActive(false);

			if (destroy)
				Destroy(gameObject);
			else
				Destroy(this);
		}
	}
}
