using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarUpdater : MonoBehaviour {
	
	public GameObject infoObject;
	
	[Space]
    public bool forHp = true;
	public bool forAction = false;
	public float swayFactor = 1f;

	[Space]
	public float gainPop = 1f;
	public float gainPopFactor = 0.1f;
	public float gainPopDecayFactor = 0.25f;

	private Image barFill;

	private HitCheck hitCheck;
	private PlayerController playerController;
	private ObjectShake objectShake;

	private float hitPoints;
	private float actionPoints;

	private float barProgress = 0f;

	private float prevValue;
	private float valueDiff = 0f;
	private Vector3 prevPos;
	private Vector3 prevScale;
	
	void Awake () {
        barFill = GetComponent<Image>();
        hitCheck = infoObject.GetComponent<HitCheck>();
        playerController = infoObject.GetComponent<PlayerController>();
		prevPos = transform.parent.localPosition;
		prevScale = transform.localScale;
    }
	
	void Update () {
		if(infoObject != null)
		{
            hitPoints = hitCheck.hp;
            actionPoints = playerController.actionPoints;
			
            if (forHp)
			{    
                barFill.fillAmount = hitPoints;

				if (hitPoints > prevValue)
					valueDiff = hitPoints - prevValue;

				prevValue = hitPoints;
			}
			else if(forAction)
			{                
				if(hitPoints <= 0f)
					barProgress = 0f;
				else
                    barFill.fillAmount = actionPoints;

				if(actionPoints > prevValue)
					valueDiff = actionPoints - prevValue;

				prevValue = actionPoints;

			}
			
			transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, prevPos + (new Vector3(playerController.trackVelocity.x * swayFactor,
				playerController.trackVelocity.y * swayFactor, 0f)), 0.2f);

			transform.localScale = Vector3.Lerp(prevScale,
				new Vector3(prevScale.x + (valueDiff * gainPop), prevScale.y + (valueDiff * gainPop), prevScale.z + (valueDiff * gainPop)),
				gainPopFactor);

			valueDiff = Mathf.Lerp(valueDiff, 0f, gainPopDecayFactor);
		}
	}
}
