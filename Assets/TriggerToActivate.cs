using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToActivate : MonoBehaviour
{
	public GameObject[] objectsToActivate;
	public GameObject[] objectsToDeactivate;
	public bool selfDeactivate = true;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		for (int i = 0; i < objectsToDeactivate.GetLength(0); i++)
			objectsToDeactivate[i].SetActive(false);

		for (int i = 0; i < objectsToActivate.GetLength(0); i++)
			objectsToActivate[i].SetActive(true);

		if (selfDeactivate)
			gameObject.SetActive(false);
	}
}
