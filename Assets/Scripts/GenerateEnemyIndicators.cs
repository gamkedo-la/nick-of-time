using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemyIndicators : MonoBehaviour
{
	public float delay = 2f;

	public GameObject indicatorPrefab;

	[SerializeField] private Camera cam;
	[SerializeField] private Transform bottomLeft;
	[SerializeField] private Transform topRight;
	
	private float timer = 0f;

    void Start()
    {
		timer = delay;
    }
	
    void Update()
    {
		//Instantiate a Prefab [with Sprite (dim triangle) and IndicateTowards] setted to Enemy and those two transforms from here
		//Have disable render when near enough to player

		if (timer <= 0f)
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
				Destroy(gameObject.transform.GetChild(i));

			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

			if (enemies != null)
			{
				foreach (var e in enemies)
				{
					GameObject indicator = Instantiate(indicatorPrefab, gameObject.transform);
					indicator.layer = gameObject.layer;
					IndicateTowards indTow = indicator.GetComponent<IndicateTowards>();
					indTow.objectToIndicate = e;
					indTow.cam = cam;
					indTow.bottomLeft = bottomLeft;
					indTow.topRight = topRight;
				}
			}

			timer = delay;
		}
		else
		{
			timer -= Time.deltaTime;
		}
    }
}
