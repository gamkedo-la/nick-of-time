using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarUpdater : MonoBehaviour {
	
	public GameObject infoObject;
	
	public bool forHp = true;
	public bool forAction = false;

	private float barProgress = 0f;
	
	void Start () {
		
	}
	
	void Update () {
		if(infoObject != null)
		{
			if(forHp)
			{
				barProgress = (infoObject.GetComponent<HitCheck>().hp + (-0.63f * (infoObject.GetComponent<HitCheck>().hp - 1f)));
			}
			else if(forAction)
			{
				if(infoObject.GetComponent<HitCheck>().hp <= 0f)
					barProgress = 0f;
				else
					barProgress = (infoObject.GetComponent<PlayerController>().actionPoints + (-0.63f * (infoObject.GetComponent<PlayerController>().actionPoints - 1f)));
			}
		}
	}
}
