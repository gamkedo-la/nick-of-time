using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HitCheck : MonoBehaviour
{
	public float defenseFactor = 1f;
	public float thrownObjectDamage = 0.25f;
	public bool breakAttackOnHit = true;

	private float criticalDamageRatio = 2f;
	private float criticalKnockbackRatio = 1.5f;

	public string[] hitTags;

	[Space]
	public float knockbackSlowDown = 0.001f;

	[Space]
	public float invulnerabilityDelay = 0f;

	[Space]
	public float regenerateHpUpto = 0f;
	public float regenerateHpPerSecond = 0.0025f;

	[Space]
	public AudioClip damageSound;
	public AudioClip deathSound;
	public AudioClip lowHpSound;

	[Space]
	public int lootIterations = 0;
	public float lootChance = 0f;
	public GameObject[] lootItems = null;
	public GameObject[] hitParticle;
	public GameObject[] criticalParticle;

	[HideInInspector] public float hp = 1f;
	[HideInInspector] public Vector2 knockback = Vector2.zero;
	[HideInInspector] public Vector2 knockbackSlowDownVector = Vector2.zero;
	
	private bool isHit = false;
	private float hpDamage = 0f;
	private int hitDirection = 0;

	private float hitAngle = 0f;
	private float knockbackValue = 0f;

	private AudioSource aud = null;
	
	[Space]
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

	private float invulnerabilityTimer = 0f;

	private bool critical = false;

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
		if (isHit)
		{
			hp -= hpDamage;

			invulnerabilityTimer = invulnerabilityDelay;

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

			knockback = new Vector2(knockbackValue * Mathf.Cos(hitAngle), knockbackValue * Mathf.Sin(hitAngle));
			knockbackSlowDownVector = new Vector2(knockbackSlowDown * Mathf.Cos(hitAngle), knockbackSlowDown * Mathf.Sin(hitAngle));
			
			isHit = false;

			if (aud != null && TogglesValues.sound)
				aud.PlayOneShot(damageSound);

			FloatingTextService.Instance.ShowFloatingTextStandard(transform.position,
				Mathf.RoundToInt(hpDamage * 100f).ToString(), critical ? Color.yellow : Color.white,
				critical ? 2f : 1f, critical ? 2f : 1f);

			if (hitParticle != null && hitParticle.Length > 0)
			{
				Instantiate(
						critical ? criticalParticle[Random.Range(0, criticalParticle.Length)]
						: hitParticle[Random.Range(0, hitParticle.Length)],
					transform.position, Quaternion.Euler(0f, 0f, 0f));
			}
			
			critical = false;
		}
		else
		{
			invulnerabilityTimer -= Time.deltaTime;
		}

		if (hp <= 0f)
		{
			if ((GetComponent<Animator>() != null && !GetComponent<Animator>().GetBool("isDying")) && aud != null && TogglesValues.sound)
			{
				aud.clip = deathSound;
				aud.PlayDelayed(0.5f);

				if (lootChance > 0f)
				{
					for (int i = 0; i < lootIterations; i++)
					{
						if (Random.Range(0f, 1f) >= lootChance)
						{
							Instantiate(lootItems[Random.Range(0, lootItems.Length)], transform.position, Quaternion.Euler(0f, 0f, 0f));
						}
					}
				}
			}

			if (GetComponent<Animator>() != null)
			{
				GetComponent<Animator>().SetBool("isDying", true);

				Destroy(GetComponent<Collider2D>());
				if (hpSlider) Destroy(hpSlider.gameObject.transform.parent.gameObject);
			}
			else
			{
				Destroy(GetComponent<BlinkEffect>());
				Destroy(GetComponent<EnemyAI>());
				Destroy(GetComponent<HitCheck>());

				Destroy(GetComponent<Collider2D>());
				if (hpSlider) Destroy(hpSlider.gameObject.transform.parent.gameObject);

				gameObject.AddComponent<SpriteFadeOut>().delay = 1f;
			}
		}
		else if (hp < regenerateHpUpto)
		{
			hp += regenerateHpPerSecond * Time.deltaTime;
		}
		else if (hp > maxHp)
		{
			hp -= Time.deltaTime / 10f;
		}

		if (hp < 0.25f && lowHpSound != null)
		{
			if (aud != null && TogglesValues.sound && !aud.isPlaying)
				aud.PlayOneShot(lowHpSound);
		}

	}

	void OnTriggerStay2D( Collider2D coll )
	{
		if (invulnerabilityTimer <= 0f)
		{
			for (int i = 0; i < hitTags.Length; i++)
			{
				if (coll.gameObject.tag == hitTags[i])
				{
					ThrownObject to = coll.gameObject.GetComponent<ThrownObject>();

					if (coll.gameObject.GetComponent<DamageObject>() != null
					|| (coll.gameObject.name.Contains("EM_ATT") && (coll.gameObject.transform.parent != null && coll.gameObject.transform.parent.parent != null && coll.gameObject.transform.parent.parent != null && (coll.gameObject.transform.parent.parent.parent.GetComponent<EnemyAI>().isAttacking || coll.gameObject.transform.parent.parent.parent.GetComponent<Animator>().GetBool("isAttacking")))))
					{
						if (PlayerDamage(coll)) break;
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
	}

	private bool PlayerDamage(Collider2D coll)
	{
		if (gameObject.name.Contains("Player"))
		{
			if (!gameObject.GetComponent<PlayerController>().isDashing)
			{
				DamageObject dmg = coll.gameObject.GetComponent<DamageObject>();

				if (dmg != null)
				{
					hpDamage = dmg.hpDamage / defenseFactor;
					knockbackValue = 0f;
				}
				else
				{
					hpDamage = coll.gameObject.transform.parent.parent.parent.GetComponent<EnemyAI>().damageToPlayer / defenseFactor;
					knockbackValue = 4.0f;
				}

				if (breakAttackOnHit)
					GetComponent<PlayerController>().stopAttacking();

				isHit = true;
				OnHit.Invoke( );
				hitAngle = Mathf.Atan2(gameObject.transform.position.y - coll.gameObject.transform.position.y,
				coll.gameObject.transform.position.x - coll.gameObject.transform.position.x);
				return true;
			}
		}
		return false;
	}

	private bool EnemyDamageOnThrownObject(Collider2D coll, ThrownObject to)
	{
		if (coll.gameObject.name.Contains("PL_ATT"))
		{
			hpDamage = thrownObjectDamage / defenseFactor;

			//no knockback on thrown object
			knockbackValue = 0f;

			if (breakAttackOnHit)
				GetComponent<EnemyAI>().stopAttacking();

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
			PlayerController plCont = coll.gameObject.transform.parent.parent.parent.gameObject.GetComponent<PlayerController>();
			if (!plCont.isDashing)
			{
				plCont.didAttackHitEnemy++;

				critical = plCont.processCriticalHit();
				
				if (plCont.playerNo == 1)
					ArenaScoreSystem.AddPlayer1Score(5 * (critical ? 2 : 1)
					* (plCont.hitComboCount == 0 ? 1 : plCont.hitComboCount));
				else if (plCont.playerNo == 2)
					ArenaScoreSystem.AddPlayer2Score(5 * (critical ? 2 : 1)
					* (plCont.hitComboCount == 0 ? 1 : plCont.hitComboCount));
				
				if (plCont.weaponPossession.weaponID < 0)
				{
					hpDamage = plCont.weaponPossession.defaultDamage
					* (critical ? criticalDamageRatio : 1f);

					knockbackValue = plCont.weaponPossession.defaultKnockback
					* (critical ? criticalKnockbackRatio : 1f);
				}
				else
				{
					hpDamage = (plCont.weaponPossession.weapons[plCont.weaponPossession.weaponID].damage
					* (critical ? criticalDamageRatio : 1f)) / defenseFactor;

					knockbackValue = (plCont.weaponPossession.weapons[plCont.weaponPossession.weaponID].knockback
					* (critical ? criticalKnockbackRatio : 1f)) / defenseFactor;
				}

				if (breakAttackOnHit)
					GetComponent<EnemyAI>().stopAttacking();

				isHit = true;
				OnHit.Invoke( );
				hitAngle = Mathf.Atan2(coll.gameObject.transform.parent.parent.parent.position.y - gameObject.transform.position.y,
				coll.gameObject.transform.parent.parent.parent.position.x - gameObject.transform.position.x);
				return true;
			}
		}
		return false;
	}

	public void die()
	{
		if (gameObject.CompareTag("Enemy")
		&& gameObject.name.Contains("FireSnake"))
			Destroy(gameObject.GetComponent<EnemyAI>().pieces);

		Destroy(gameObject);
	}
}
