using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NickBossScript : MonoBehaviour
{
	public int maxSpawn = 6;
	public int totalSpawns = 150;
	public float delay = 1f;

	public Transform bossTransform;
	public GameObject[] enemyObjects;
	public Transform[] enemySpawnPoints;
	public GameObject explosion;
	public TextMeshPro hpDisplay;

	private List<GameObject> enemies = new List<GameObject>();

	private float timer = 0f;

    void Start()
    {
        
    }
	
    void Update()
    {
		if (timer <= 0f && enemies.Count < maxSpawn && totalSpawns > 0)
		{
			enemies.Add(Instantiate(enemyObjects[Random.Range(0, enemyObjects.Length)], enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length)].position, Quaternion.Euler(0f, 0f, 0f)));
			totalSpawns--;
			hpDisplay.text = totalSpawns.ToString();
			timer = delay;
		}
		else if(timer > 0f)
		{
			timer -= Time.deltaTime;
		}

		for (int i = 0; i < enemies.Count; i++)
		{
			if (enemies[i] == null)
				enemies.RemoveAt(i);
		}

		if (totalSpawns <= 0 && enemies.Count <= 0)
		{
			Instantiate(explosion, bossTransform.position, Quaternion.Euler(0f,0f,0f));
			Destroy(gameObject);
		}
    }
}
