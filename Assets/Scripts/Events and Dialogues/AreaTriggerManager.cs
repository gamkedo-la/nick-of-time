using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerManager : MonoBehaviour
{

	[System.Serializable]
	public class AreaObject
	{
		public GameObject obj = null;
		public bool disableAfterLeaving = false;
	};

	[System.Serializable]
	public class AreaTrigger
	{
		[Header("Area Trigger")]
		[Space]
		public Transform triggerTransform;

		[Space]
		public bool checkEnemies = false;
		public bool destroyEnemies = false;

		[Space]
		public bool isCounted = true;

		[Space]
		public AreaObject[] objectToActivate;

		[Space]
		public GameObject enemySpawnObject = null;

		[HideInInspector] public float triggerTimer = 0f;
		[HideInInspector] public bool isObjectsActivationDone = false;
		[HideInInspector] public bool isEnemySpawnDone = false;

		[HideInInspector] public int decrementTriggerCount = -1;
	};

	public AreaTrigger[] triggers;

	private CameraTriggerControl[] camTriggerCont1;
	private CameraTriggerControl[] camTriggerCont2;

	void Start()
	{
		camTriggerCont1 = new CameraTriggerControl[triggers.Length];
		camTriggerCont2 = new CameraTriggerControl[triggers.Length];

		for (int i = 0; i < triggers.Length; i++)
		{
			if (triggers[i].isCounted)
				LevelManager.triggerCount++;

			triggers[i].triggerTimer = 0f;
			triggers[i].isObjectsActivationDone = false;
			triggers[i].isEnemySpawnDone = false;

			triggers[i].decrementTriggerCount = -1;

			//Every Camera Trigger Transform MUST have 2 CameraTriggerControl (1 for each player)
			camTriggerCont1[i] = triggers[i].triggerTransform.gameObject.GetComponents<CameraTriggerControl>()[0];
			camTriggerCont2[i] = triggers[i].triggerTransform.gameObject.GetComponents<CameraTriggerControl>()[1];
		}
	}

	void Update()
	{
		bool areThereEnemies = GameObject.FindWithTag("Enemy") != null;

		for (int ti = 0; ti < triggers.Length; ti++)
		{
			AreaTrigger t = triggers[ti];

			if (t.decrementTriggerCount == 0)
			{
				LevelManager.triggerCount--;
				t.decrementTriggerCount = -1;
			}
			else if (t.decrementTriggerCount > 0)
			{
				t.decrementTriggerCount--;
			}

			int triggerCount = 0;
			triggerCount += camTriggerCont1[ti].triggered ? 1 : 0;
			triggerCount += camTriggerCont2[ti].triggered ? 1 : 0;

			if (!t.isObjectsActivationDone && (!t.checkEnemies || !areThereEnemies))
			{
				if (triggerCount > 0)
				{
					foreach (var obj in t.objectToActivate)
						if (obj.obj != null)
						{
							if (obj.obj.GetComponent<DisableAfterDelay>())
								Destroy(obj.obj.GetComponent<DisableAfterDelay>());

							obj.obj.SetActive(true);
						}

					t.isObjectsActivationDone = true;

					if (t.isCounted)
						t.decrementTriggerCount = 100;

					if (t.destroyEnemies)
					{
						GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

						for (int i = 0; i < enemies.GetLength(0); i++)
						{
							Destroy(enemies[i]);
						}
					}
				}
			}
			else if (triggerCount > 0)
			{
				foreach (var obj in t.objectToActivate)
				{
					if (obj.obj != null)
					{
						if (obj.disableAfterLeaving)
						{
							if (obj.obj.GetComponent<DisableAfterDelay>())
								Destroy(obj.obj.GetComponent<DisableAfterDelay>());

							obj.obj.SetActive(true);
						}
					}
				}
			}
			else if (triggerCount <= 0)
			{
				foreach (var obj in t.objectToActivate)
				{
					if (obj.obj != null)
					{
						if (obj.disableAfterLeaving)
							if(obj.obj.activeSelf && obj.obj.GetComponent<DisableAfterDelay>() == null)
								obj.obj.AddComponent<DisableAfterDelay>().delay = 0.6f;
								//obj.obj.SetActive(false);
					}
				}
			}

			if (t.enemySpawnObject != null && !t.isEnemySpawnDone)
			{
				for (int c = 0; c < t.triggerTransform.gameObject.GetComponents<CameraTriggerControl>().GetLength(0); c++)
				{
					if (t.triggerTransform.gameObject.GetComponents<CameraTriggerControl>()[c].triggered)
					{
						for (int i = 0; i < t.triggerTransform.transform.childCount; i++)
						{
							Instantiate(t.enemySpawnObject, t.triggerTransform.transform.GetChild(i).transform.position, Quaternion.Euler(0f, 0f, 0f));
						}

						t.isEnemySpawnDone = true;

						/*
						if (t.isCounted)
							t.decrementTriggerCount = 100;
							*/
					}
				}
			}
		}
	}
}
