using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnSinglePlayerGame : MonoBehaviour {

	void Start () {
		if(GameManager.singleGame)
			Destroy(gameObject);
	}
	
	void Update () {
		
	}
}
