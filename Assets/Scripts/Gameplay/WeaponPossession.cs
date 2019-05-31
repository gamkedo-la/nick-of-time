using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPossession : MonoBehaviour {
	
	public int weaponID = -1;

	[System.Serializable]
	public class WeaponProperties
	{
		public Sprite sprite;
		public Vector2 colliderSize;
	}
	
	public WeaponProperties[] weapons;

	public float offsetX = 0.1f;

	private int prevWeaponID = -1;
	
	private SpriteRenderer sprRenderer;
	private BoxCollider2D collider;
	private SpriteRenderer playerRenderer;
	
	//0 = Dagger

	void Start () {
		sprRenderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();
		playerRenderer = transform.parent.parent.parent.GetComponent<SpriteRenderer>();
	}
	
	void Update () {
		if(weaponID != prevWeaponID)
		{
			if (weaponID <= -1)
			{
				sprRenderer.sprite = null;
				collider.size = new Vector2(0.03f, 0.03f);
			}
			else
			{
				sprRenderer.sprite = weapons[weaponID].sprite;
				collider.size = weapons[weaponID].colliderSize;
			}
			
			prevWeaponID = weaponID;
		}
		
		/*
		Vector3 sc = transform.parent.localScale;
		sc.x = playerRenderer.flipX == true ? -1f : 1f;
		transform.parent.localScale = sc;
		*/

		/*
		Vector3 pos = transform.parent.localPosition;
		pos.x = playerRenderer.flipX == true ? offsetX : 0f;
		transform.parent.localPosition = pos;
		*/
	}
}
