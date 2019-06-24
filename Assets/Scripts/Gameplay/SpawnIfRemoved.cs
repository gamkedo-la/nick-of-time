using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnIfRemoved : MonoBehaviour
{
	private GameObject inactiveFirstChildObject;

	[SerializeField] private int spawnLimit = 1;
	[SerializeField] private float spawnDelay = 0f;
	[SerializeField] private float spawnStartDelay = 0f;

	private float spawnTimer = 0f;

	void Start()
    {
		inactiveFirstChildObject = transform.GetChild(0).gameObject;

		spawnTimer = spawnStartDelay;
    }
	
    void Update()
    {
		if (transform.childCount < 1 + spawnLimit && spawnTimer <= 0f)
		{
			GameObject obj = Instantiate(inactiveFirstChildObject, transform.position, inactiveFirstChildObject.transform.rotation);
			obj.SetActive(true);
			obj.transform.parent = gameObject.transform;

			spawnTimer = spawnDelay;
		}

		spawnTimer -= Time.deltaTime;
    }
}
