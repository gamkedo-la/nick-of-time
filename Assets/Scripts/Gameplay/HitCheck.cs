﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[HideInInspector] public float hp = 1f;
	[HideInInspector] public Vector2 knockback = Vector2.zero;

	private bool isHit = false;
	private bool hitLeft = false;

	private AudioSource aud = null;

    [SerializeField]
    private GameObject HPBarJointCamera;
    [SerializeField]
    private GameObject HPBarSoloCamera;

	void Start ()
	{
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
    }

	void Update ()
	{
		if(isHit)
		{
			hp -= hpDamage;
			//Debug.Log("current HP is " + hp);

			if (HPBarSoloCamera != null)
			{
				StartCoroutine(HPBarJointCamera.GetComponent<ObjectShake>().Shake(10f, 0.2f)); //Shakes HP Bar on Hit
				StartCoroutine(HPBarSoloCamera.GetComponent<ObjectShake>().Shake(10f, 0.2f)); //Shakes HP Bar on Hit
			}

            knockback = new Vector2(knockbackValue, 0f);
			knockbackSlowDown = Mathf.Abs(knockbackSlowDown);
			if(hitLeft)
			{
				knockback = new Vector2(-knockbackValue, 0f);
				knockbackSlowDown = -knockbackSlowDown;
			}

			isHit = false;

			if(aud != null && TogglesValues.sound)
				aud.PlayOneShot(damageSound);

			FloatingTextService.Instance.ShowFloatingTextStandard( transform.position, hpDamage.ToString( ), Color.red );
		}

		if(hp <= 0f)
		{
			if(!GetComponent<Animator>().GetBool("isDying") && aud != null && TogglesValues.sound)
			{
				aud.clip = deathSound;
				aud.PlayDelayed(0.5f);
			}

			GetComponent<Animator>().SetBool("isDying", true);
		}
		else if(hp < regenerateHpUpto)
		{
			hp += regenerateHpPerSecond * Time.deltaTime;
		}

	}

	void OnTriggerStay2D( Collider2D coll )
	{
		for(int i = 0; i < hitTags.GetLength(0); i++)
		{
			if(coll.gameObject.CompareTag(hitTags[i])
				&& coll.gameObject.transform.parent.parent.parent.GetComponent<Animator>().GetBool("isAttacking")
				&& !gameObject.GetComponent<Animator>().GetBool("isRolling"))
			{
				isHit = true;
				hitLeft = gameObject.transform.position.x < coll.gameObject.transform.position.x;// coll.gameObject.GetComponent<SpriteRenderer>().flipX;
				break;
			}
		}
	}

	public void die()
	{
		Destroy(gameObject);
	}
}
