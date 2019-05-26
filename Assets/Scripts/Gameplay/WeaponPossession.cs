using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPossession : MonoBehaviour {
	
	public int weaponID = -1;
	
	public Sprite[] weapons;

	public float offsetX = 0.1f;

	private int prevWeaponID = -1;
	
	private SpriteRenderer sprRenderer;

	private SpriteRenderer playerRenderer;
	
	//0 = Dagger

	void Start () {
		sprRenderer = GetComponent<SpriteRenderer>();
		playerRenderer = transform.parent.parent.parent.GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if(weaponID != prevWeaponID)
		{
			sprRenderer.sprite = weapons[weaponID];
			
			prevWeaponID = weaponID;
		}
		
		/*
		Vector3 sc = transform.parent.localScale;
		sc.x = playerRenderer.flipX == true ? -1f : 1f;
		transform.parent.localScale = sc;
		*/

		Vector3 pos = transform.parent.localPosition;
		pos.x = playerRenderer.flipX == true ? offsetX : 0f;
		transform.parent.localPosition = pos;
	}
}
