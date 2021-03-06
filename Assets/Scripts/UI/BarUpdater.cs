﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BarUpdater : MonoBehaviour {
	
	public GameObject infoObject;
	
	[Space]
    public bool forHp = true;
	public bool forAction = false;
	public float swayFactor = 1f;
	public float trackVelocityThreshold = 1f;

	[Space]
	public float gainPop = 1f;
	public float gainPopFactor = 0.1f;
	public float gainPopDecayFactor = 0.25f;

    // when value is low, the bar flashes on and off
	[Space]
	public float flashInterval = 16; // frames per flash on and off
	public float flashIfHpBelow = 0.25f; // max amount
	public float flashIfApBelow = 0.25f; // max amount


	private Image barFill;

	private HitCheck hitCheck;
	private PlayerController playerController;
	private ObjectShake objectShake;

	private float hitPoints;
	private float actionPoints;

	private float barProgress = 0f;

	private float prevValue = 1f;
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

	private void Start()
	{
		prevPos = transform.parent.localPosition;
		prevScale = transform.localScale;
	}

	void Update () {
		if(infoObject != null)
		{
			bool flashing = false;
			
			hitPoints = hitCheck.hp;
            actionPoints = playerController.actionPoints;
			
            if (forHp)
			{    
                barFill.fillAmount = hitPoints;

				if (hitPoints > prevValue)
					valueDiff = hitPoints - prevValue;

				prevValue = hitPoints;

				flashing = hitPoints < flashIfHpBelow; 
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

				flashing = actionPoints < flashIfApBelow; 
			}
			
			if(Mathf.Abs(playerController.trackVelocity.x) <= trackVelocityThreshold && Mathf.Abs(playerController.trackVelocity.y) <= trackVelocityThreshold)
				transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, prevPos + (new Vector3(playerController.trackVelocity.x * swayFactor,
					playerController.trackVelocity.y * swayFactor, 0f)), 0.2f);
			else
					transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, prevPos, 0.2f);

			transform.localScale = Vector3.Lerp(prevScale,
				new Vector3(prevScale.x + (valueDiff * gainPop), prevScale.y + (valueDiff * gainPop), prevScale.z + (valueDiff * gainPop)),
				gainPopFactor);

			if (flashing) {
				if ((Time.frameCount % flashInterval) > flashInterval/2) // alternate
					transform.localScale = Vector3.one;
			}

			valueDiff = Mathf.Lerp(valueDiff, 0f, gainPopDecayFactor);
		}
	}
}
