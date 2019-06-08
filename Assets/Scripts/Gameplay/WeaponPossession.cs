using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPossession : MonoBehaviour {
	
	public int weaponID = -1;

	[System.Serializable]
	public class WeaponProperties
	{
		public Sprite sprite;
		public Vector2 position;
		public Vector2 colliderOffset;
		public Vector2 colliderSize;
	}
	
	public WeaponProperties[] weapons;

	public float offsetX = 0.1f;

	private int prevWeaponID = -10;
	
	private SpriteRenderer sprRenderer;
	private BoxCollider2D collider;
	//private SpriteRenderer playerRenderer;
	
	//0 = Dagger
	//1 = Katana

	void Start ()
	{
		sprRenderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();
		//playerRenderer = transform.parent.parent.parent.GetComponent<SpriteRenderer>();
	}

	void Update ()
	{
		if(weaponID != prevWeaponID)
		{
			if (weaponID <= -1)
			{
				sprRenderer.sprite = null;
				collider.offset = Vector2.zero;
				collider.size = new Vector2(0.03f, 0.03f);
				transform.localPosition = new Vector2(-0.0744f, -0.045f);
			}
			else
			{
				sprRenderer.sprite = weapons[weaponID].sprite;
				collider.offset = weapons[weaponID].colliderOffset;
				collider.size = weapons[weaponID].colliderSize;
				transform.localPosition = weapons[weaponID].position;
			}
			
			prevWeaponID = weaponID;
		}
	}
}
