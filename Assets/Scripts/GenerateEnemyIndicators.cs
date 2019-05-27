using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//	WORK IN PROGRESS...

public class GenerateEnemyIndicators : MonoBehaviour
{
	public GameManager indicatorPrefab;

	[SerializeField] private Transform bottomLeft;
	[SerializeField] private Transform topRight;

    void Start()
    {
        
    }
	
    void Update()
    {
        //Instantiate a Prefab [with Sprite (dim triangle) and IndicateTowards] setted to Enemy and those two transforms from here
		//Have disable render when near enough to player
    }
}
