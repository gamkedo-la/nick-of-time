﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HitCheck : MonoBehaviour
{
	public float hpDamage = 0.04f;
	public string[] hitTags;
	public float knockbackValue = 0.48f;
	public float knockbackSlowDown = 0.001f;

	public float regenerateHpUpto = 0f;
	public float regenerateHpPerSecond = 0.0025f;

	public AudioClip damageSound;
	public AudioClip deathSound;

	public int lootIterations = 0;
	public float lootChance = 0f;
	public GameObject[] lootItems = null;

	[HideInInspector] public float hp = 1f;
	[HideInInspector] public Vector2 knockback = Vector2.zero;

	private bool isHit = false;
	private int hitDirection = 0;

	private AudioSource aud = null;

    [SerializeField]
    private GameObject HPBarJointCamera;
    [SerializeField]
    private GameObject HPBarSoloCamera;
    [SerializeField]
    private GameObject HPBarEnemy;

	public UnityEvent OnHit = null;

	[SerializeField]
	private SpriteSlider hpSlider;

	private float maxHp = 1f;

	void Start ()
	{
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();

		maxHp = hp;

		if (hpSlider)
		{
			hpSlider.fillValue = hp / maxHp;
		}
    }

	void Update ()
	{
		if(isHit)
		{
			hp -= hpDamage;

			if (hpSlider)
				hpSlider.fillValue = hp / maxHp;

			if (HPBarSoloCamera != null)
			{
				if (HPBarEnemy)
					StartCoroutine(HPBarEnemy.GetComponent<ObjectShake>().Shake(10f, 0.2f));
				else
				{
					StartCoroutine(HPBarJointCamera.GetComponent<ObjectShake>().Shake(10f, 0.2f));
					StartCoroutine(HPBarSoloCamera.GetComponent<ObjectShake>().Shake(10f, 0.2f));
				}
			}

			if (hitDirection == 0)
			{
				knockback = new Vector2(0f, -knockbackValue);
				knockbackSlowDown = -Mathf.Abs(knockbackSlowDown);
			}
			else if (hitDirection == 1)
			{
				knockback = new Vector2(-knockbackValue, 0f);
				knockbackSlowDown = -Mathf.Abs(knockbackSlowDown);
			}
			else if(hitDirection == 2)
			{
				knockback = new Vector2(0f, knockbackValue);
				knockbackSlowDown = Mathf.Abs(knockbackSlowDown);
			}
			else if(hitDirection == 3)
			{
				knockback = new Vector2(knockbackValue, 0f);
				knockbackSlowDown = Mathf.Abs(knockbackSlowDown);
			}

			isHit = false;

			if(aud != null && TogglesValues.sound)
				aud.PlayOneShot(damageSound);

			FloatingTextService.Instance.ShowFloatingTextStandard( transform.position, hpDamage.ToString( ), Color.red );
		}

		if (hp <= 0f)
		{
			if (!GetComponent<Animator>().GetBool("isDying") && aud != null && TogglesValues.sound)
			{
				aud.clip = deathSound;
				aud.PlayDelayed(0.5f);

				if (lootChance > 0f)
				{
					for (int i = 0; i < lootIterations; i++)
					{
						if (Random.Range(0f, 1f) >= lootChance)
						{
							Instantiate(lootItems[Random.Range(0, lootItems.Length)], transform.position, Quaternion.Euler(0f,0f,0f));
						}
					}
				}
			}

			GetComponent<Animator>().SetBool("isDying", true);
		}
		else if (hp < regenerateHpUpto)
		{
			hp += regenerateHpPerSecond * Time.deltaTime;
		}
		else if (hp > maxHp)
		{
			hp -= Time.deltaTime / 10f;
		}

	}

	void OnTriggerStay2D( Collider2D coll )
	{
		for(int i = 0; i < hitTags.Length; i++)
		{
			if (coll.gameObject.tag == hitTags[i])
			{
				ThrownObject to = coll.gameObject.GetComponent<ThrownObject>();

				if (coll.gameObject.GetComponent<DamageObject>() != null
				|| (coll.gameObject.name.Contains("EM_ATT") && (coll.gameObject.transform.parent != null && coll.gameObject.transform.parent.parent != null && coll.gameObject.transform.parent.parent != null && coll.gameObject.transform.parent.parent.parent.GetComponent<Animator>().GetBool("isAttacking"))))
				{
					if (PlayerDamage()) break;
				}
				else if (to != null && to.throwVelocity != Vector3.zero)
				{
					if (EnemyDamageOnThrownObject(coll, to)) break;
				}
				else if (coll.gameObject.transform.parent != null && coll.gameObject.transform.parent.parent != null && coll.gameObject.transform.parent.parent != null && coll.gameObject.transform.parent.parent.parent.GetComponent<Animator>().GetBool("isAttacking"))
				{
					if (EnemyDamage(coll)) break;
				}
			}
		}
	}

	private bool PlayerDamage()
	{
		if (gameObject.name.Contains("Player"))
		{
			if (!gameObject.GetComponent<PlayerController>().isDashing)
			{
				Animator anim = gameObject.GetComponent<Animator>();

				isHit = true;
				OnHit.Invoke( );
				hitDirection = anim.GetInteger("direction");
				return true;
			}
		}
		return false;
	}

	private bool EnemyDamageOnThrownObject(Collider2D coll, ThrownObject to)
	{
		if (coll.gameObject.name.Contains("PL_ATT"))
		{
			isHit = true;
			OnHit.Invoke( );
			hitDirection = to.GetDirection();
			return true;
		}
		return false;
	}

	private bool EnemyDamage(Collider2D coll)
	{
		if (coll.gameObject.name.Contains("PL_ATT"))
		{
			if (!coll.gameObject.transform.parent.parent.parent.gameObject.GetComponent<PlayerController>().isDashing)
			{
				Animator anim = coll.gameObject.transform.parent.parent.parent.gameObject.GetComponent<Animator>();

				isHit = true;
				OnHit.Invoke( );
				hitDirection = anim.GetInteger("direction");
				return true;
			}
		}
		return false;
	}

	public void die()
	{
		Destroy(gameObject);
	}
}