using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPossession : MonoBehaviour {
	
	public int weaponID = -1;

	[System.Serializable]
	public class AnimatedWeaponProperties
	{
		public bool animated = false;
		public Vector2 attackUpPosition;
		public Vector2 attackDownPosition;
		public Vector2 attackLeftPosition;
		public Vector2 attackRightPosition;
		public Sprite[] attackUp;
		public Sprite[] attackDown;
		public Sprite[] attackLeft;
		public float timePerFrame;
		public bool flipAttackLeft;
		public bool turnOffSingleSpriteRenderer;
	}

	[System.Serializable]
	public class WeaponProperties
	{
		public float damage;
		public float knockback;
		public Sprite sprite;
		public Vector2 position;
		public Vector2 colliderOffset;
		public Vector2 colliderSize;

		public AnimatedWeaponProperties animatedWeapon = null;
	}

	public float defaultDamage = 0.05f;
	public float defaultKnockback = 2f;
	
	public WeaponProperties[] weapons;

	public SpriteRenderer animatedWeaponRenderer;

	public float offsetX = 0.1f;

	private int prevWeaponID = -10;
	
	private SpriteRenderer sprRenderer;
	private BoxCollider2D collider;
	private Animator playerAnimator;

	private int frame = -1;
	private float frameTimer = 0f;
	
	//0 = Dagger
	//1 = Katana
	//2 = Time Whip
	//3 - Spear

	void Start ()
	{
		sprRenderer = GetComponent<SpriteRenderer>();
		collider = GetComponent<BoxCollider2D>();

		if(GetComponent<ThrownObject>() == null)
			playerAnimator = transform.parent.parent.parent.GetComponent<Animator>();
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
				if (animatedWeaponRenderer) animatedWeaponRenderer.sprite = null;
			}
			else
			{
				sprRenderer.sprite = weapons[weaponID].sprite;
				collider.offset = weapons[weaponID].colliderOffset;
				collider.size = weapons[weaponID].colliderSize;
				transform.localPosition = weapons[weaponID].position;
				if(animatedWeaponRenderer) animatedWeaponRenderer.sprite = null;
			}
			
			prevWeaponID = weaponID;
		}

		if (weaponID > -1 && weapons[weaponID].animatedWeapon.animated && GetComponent<ThrownObject>() == null)
		{
			if (playerAnimator.GetBool("isAttacking"))
			{
				if (frame <= -1 || frameTimer >= weapons[weaponID].animatedWeapon.timePerFrame)
				{
					frame++;
					frameTimer = 0f;
				}

				int dir = playerAnimator.GetInteger("direction");

				if (weapons[weaponID].animatedWeapon.turnOffSingleSpriteRenderer)
					sprRenderer.sprite = null;

				if (dir == 0)
				{
					if (frame >= weapons[weaponID].animatedWeapon.attackUp.Length) frame = weapons[weaponID].animatedWeapon.attackUp.Length - 1;

					animatedWeaponRenderer.sprite = weapons[weaponID].animatedWeapon.attackUp[frame];
					animatedWeaponRenderer.gameObject.transform.localPosition = weapons[weaponID].animatedWeapon.attackUpPosition;
				}
				else if (dir == 1)
				{
					if (frame >= weapons[weaponID].animatedWeapon.attackLeft.Length) frame = weapons[weaponID].animatedWeapon.attackLeft.Length - 1;

					animatedWeaponRenderer.sprite = weapons[weaponID].animatedWeapon.attackLeft[frame];
					animatedWeaponRenderer.gameObject.transform.localPosition = weapons[weaponID].animatedWeapon.attackRightPosition;
				}
				else if (dir == 2)
				{
					if (frame >= weapons[weaponID].animatedWeapon.attackDown.Length) frame = weapons[weaponID].animatedWeapon.attackDown.Length - 1;

					animatedWeaponRenderer.sprite = weapons[weaponID].animatedWeapon.attackDown[frame];
					animatedWeaponRenderer.gameObject.transform.localPosition = weapons[weaponID].animatedWeapon.attackDownPosition;
				}
				else if (dir == 3)
				{
					if (frame >= weapons[weaponID].animatedWeapon.attackLeft.Length) frame = weapons[weaponID].animatedWeapon.attackLeft.Length - 1;

					animatedWeaponRenderer.sprite = weapons[weaponID].animatedWeapon.attackLeft[frame];
					animatedWeaponRenderer.gameObject.transform.localPosition = weapons[weaponID].animatedWeapon.attackLeftPosition;
				}

				if ((dir == 3 && weapons[weaponID].animatedWeapon.flipAttackLeft) || (dir == 1 && !weapons[weaponID].animatedWeapon.flipAttackLeft))
					animatedWeaponRenderer.flipX = true;
				else
					animatedWeaponRenderer.flipX = false;
			}
			else
			{
				if (weapons[weaponID].animatedWeapon.turnOffSingleSpriteRenderer)
					sprRenderer.sprite = weapons[weaponID].sprite;

				animatedWeaponRenderer.sprite = null;
				frame = -1;
			}

			frameTimer += Time.deltaTime;
		}
	}
}
