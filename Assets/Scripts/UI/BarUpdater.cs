using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarUpdater : MonoBehaviour {
	
	public GameObject infoObject;

    private Image barFill;


    private HitCheck hitCheck;
    private PlayerController playerController;
    private ObjectShake objectShake;

    private float hitPoints;
    private float actionPoints;

    public bool forHp = true;
	public bool forAction = false;

	private float barProgress = 0f;
	
	void Awake () {
        barFill = GetComponent<Image>();
        hitCheck = infoObject.GetComponent<HitCheck>();
        playerController = infoObject.GetComponent<PlayerController>();
        objectShake = GetComponent<ObjectShake>();
    }
	
	void Update () {
		if(infoObject != null)
		{
            hitPoints = hitCheck.hp;
            actionPoints = playerController.actionPoints;
            if (forHp)
			{                
                //barProgress = (hitPoints + (-0.63f * (hitPoints - 1f)));
                 Debug.Log("HIT POINTS " + hitPoints);
                barFill.fillAmount = hitPoints;
                //StartCoroutine (objectShake.Shake(0.4f, 0.4f));
                
			}
			else if(forAction)
			{                
				if(hitPoints <= 0f)
					barProgress = 0f;
				else
					//barProgress = (actionPoints + (-0.63f * (actionPoints - 1f)));
                    barFill.fillAmount = actionPoints;
            }
		}
	}
}
