using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepInBetweenScenes : MonoBehaviour {
	
	public int id = -1;
	static public int TOTAL_IDS = 100;
	static public int[] count = null;

	void Awake () {
		if(count == null)
		{
			count = new int[TOTAL_IDS];
			
			for(int i = 0; i < TOTAL_IDS; i++)
			{
				count[i] = 0;
			}
		}
		
		if(count[id] >= 1)
		{
			Destroy(gameObject);
			count[id]--;
		}
		
		DontDestroyOnLoad(gameObject);
		count[id]++;
	}
	
	void Start () {
	}
	
	void Update () {
		
	}
}
