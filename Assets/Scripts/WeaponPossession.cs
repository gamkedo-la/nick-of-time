using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPossession : MonoBehaviour {
	
	public int weaponID = -1;
	
	public Sprite[] weapons;
	
	private int prevWeaponID = -1;
	
	private Animator animator;
	private SpriteRenderer sprRenderer;
	
	//0 = Dagger

	void Start () {
		animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
		sprRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if(weaponID != prevWeaponID)
		{
			animator.SetInteger("weaponID", weaponID);
			
			sprRenderer.sprite = weapons[weaponID];
			
			prevWeaponID = weaponID;
		}
	}
}
