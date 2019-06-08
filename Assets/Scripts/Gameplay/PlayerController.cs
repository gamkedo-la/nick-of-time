using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float walkSpeed = 1f;
	public float attackSpeed = 2f;
	public float attackSpeedTime = 0.1f;
	public float dashSpeed = 3f;
	public float dashTime = 0.2f;
	public GameObject dashFXObject;
	public float pushForce = 500f;
	public float throwSpeed = 2f;

	[Space]
	public float regenerateActionPointsPerSec = 0.025f;

	[Space]
	public float attackActionDeplete = 0.4f;
	public float dashActionDeplete = 0.25f;
	public float pushActionDeplete = 0.1f;
	public float throwActionDeplete = 0.1f;

	[Space]
	public string walkVerticalInput = "Vertical";
	public string walkHorizontalInput = "Horizontal";
	public string attackInput = "Fire1";
	public string dashInput = "Jump";
	public string pushInput = "Push1";
	public string throwInput = "Throw1";

	[Space]
	public WeaponPossession weaponPossession;

	[Space]
	public AudioClip attackSound;
	public AudioClip dashSound;
	
	[HideInInspector] public float actionPoints = 1f;

	private float speed = 0f;
	private bool isDashing = false;
	private float dashTimer = 0f;
	private float attackSpeedTimer = 0f;

	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer sprRenderer;
	private HitCheck hitCheck;

	private AudioSource aud = null;
	
	private Vector2 walkInput = Vector2.zero;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprRenderer = GetComponent<SpriteRenderer>();
		hitCheck = GetComponent<HitCheck>();

		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();
	}

	private void Update()
	{
		if (Time.timeScale > 0f)
		{
			if (!animator.GetBool("isAttacking") && !animator.GetBool("isPushing") && !animator.GetBool("isThrowing") && !isDashing)
			{
				walkInput = new Vector2(Input.GetAxisRaw(walkHorizontalInput), Input.GetAxisRaw(walkVerticalInput));

				if (walkInput != Vector2.zero)
				{
					speed = walkSpeed;
					animator.SetBool("isWalking", true);

					if (walkInput.y < 0)
					{
						animator.SetInteger("direction", 2);
					}
					else if (walkInput.y > 0)
					{
						animator.SetInteger("direction", 0);
					}
					else if (walkInput.x > 0)
					{
						animator.SetInteger("direction", 1);
					}
					else if (walkInput.x < 0)
					{
						animator.SetInteger("direction", 3);
					}

					if (walkInput.x > 0) sprRenderer.flipX = true;
					else sprRenderer.flipX = false;
				}
				else
				{
					speed = 0f;
					animator.SetBool("isWalking", false);
				}
			}

			//Attack
			if (actionPoints >= attackActionDeplete
				&& !animator.GetBool("isAttacking")
				&& !animator.GetBool("isPushing")
				&& !animator.GetBool("isThrowing")
				&& !isDashing
				&& Input.GetButtonDown(attackInput))
			{                
				animator.SetBool("isAttacking", true);

				speed = attackSpeed;

				if (animator.GetBool("isWalking"))
					attackSpeedTimer = attackSpeedTime;
				else
				{
					int dir = animator.GetInteger("direction");
					if (dir == 0)
					{
						walkInput.x = 0;
						walkInput.y = 1;
					}
					else if (dir == 1)
					{
						walkInput.x = 1;
						walkInput.y = 0;
					}
					else if (dir == 2)
					{
						walkInput.x = 0;
						walkInput.y = -1;
					}
					else if (dir == 3)
					{
						walkInput.x = -1;
						walkInput.y = 0;
					}
					attackSpeedTimer = attackSpeedTime / 2f;
				}
				
				actionPoints -= attackActionDeplete;

				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(attackSound);
			}

			//Dash
			if (actionPoints >= dashActionDeplete
				&& !isDashing
				&& !animator.GetBool("isAttacking")
				&& !animator.GetBool("isPushing")
				&& !animator.GetBool("isThrowing")
				&& Input.GetButtonDown(dashInput)
				&& hitCheck.knockback == Vector2.zero)
			{
				isDashing = true;
				dashTimer = dashTime;
				speed = dashSpeed;

				animator.SetBool("isWalking", false);

				gameObject.layer = 10;

				actionPoints -= dashActionDeplete;

				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(dashSound);
			}

			/*
			if (animator.GetBool("isAttacking")
				|| isDashing)
			{
				if (walkInput == Vector2.zero)
				{
					if (sprRenderer.flipX)
					{
						walkInput.x = 1f;
					}
					else
					{
						walkInput.x = -1f;
					}
				}
			}
			*/

			//Push
			if(!animator.GetBool("isPushing")
			&& !animator.GetBool("isAttacking")
			&& !animator.GetBool("isThrowing")
			&& !isDashing
			&& Input.GetButtonDown(pushInput))
			{
				animator.SetBool("isPushing", true);
				speed = 0f;

				actionPoints -= pushActionDeplete;
			}

			//Throw
			if(!animator.GetBool("isThrowing")
			&& !animator.GetBool("isAttacking")
			&& !animator.GetBool("isPushing")
			&& !isDashing
			&& Input.GetButtonDown(throwInput))
			{
				animator.SetBool("isThrowing", true);
				speed = attackSpeed;

				attackSpeedTimer = attackSpeedTime / 2f;

				actionPoints -= throwActionDeplete;
			}

			if (actionPoints < 1f)
			{
				actionPoints += regenerateActionPointsPerSec * Time.deltaTime;
			}

			if (isDashing && dashTimer <= 0f)
			{
				isDashing = false;
				speed = walkSpeed;
				gameObject.layer = 8;
			}
			else if (dashTimer > 0f || (attackSpeedTimer > 0f && attackSpeedTimer < attackSpeedTime/3f))
			{
				GameObject ghostSpriteFX = Instantiate(dashFXObject, transform.position, Quaternion.Euler(0f,0f,0f));

				SpriteRenderer GSFX_sprRend = ghostSpriteFX.GetComponent<SpriteRenderer>();

				GSFX_sprRend.sprite = sprRenderer.sprite;

				if (name == "Player1")
				{
					GSFX_sprRend.color = Color.red;
					GSFX_sprRend.material.color = Color.red;
				}
				else
				{
					GSFX_sprRend.color = Color.green;
					GSFX_sprRend.material.color = Color.green;
				}
			}
			
			if ((animator.GetBool("isAttacking") || animator.GetBool("isThrowing")) && attackSpeedTimer <= 0f)
			{
				speed = 0f;
			}

			attackSpeedTimer -= Time.deltaTime;
			dashTimer -= Time.deltaTime;

		}
	}

	private void pushOnCollision(Collision2D collision)
	{
		if (animator.GetBool("isPushing") == true && collision.gameObject.isStatic == false)
		{
			Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
			if (rb != null)
			{
				Vector2 force = Vector2.zero;
				int dir = animator.GetInteger("direction");
				if (dir == 0)
				{
					force.x = 0;
					force.y = pushForce;
				}
				else if (dir == 1)
				{
					force.x = pushForce;
					force.y = 0;
				}
				else if (dir == 2)
				{
					force.x = 0;
					force.y = -pushForce;
				}
				else if (dir == 3)
				{
					force.x = -pushForce;
					force.y = 0;
				}
				rb.AddForce(force, ForceMode2D.Impulse);
			}

			collision.gameObject.AddComponent<PushedObject>();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		pushOnCollision(collision);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		pushOnCollision(collision);
	}

	void FixedUpdate()
	{
		if (hitCheck.hp > 0f)
		{
			rigidbody.MovePosition(new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y) + (walkInput * speed * Time.deltaTime) + ((sprRenderer.flipX == true ? -1f : 1f) * hitCheck.knockback * Time.deltaTime));
		}
		else
		{
			rigidbody.MovePosition(new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y) + ((sprRenderer.flipX == true ? -1f : 1f) * hitCheck.knockback * Time.deltaTime));

			stopAttacking();
			stopDashing();
			stopPushing();
			stopThrowing();
		}

		if (Mathf.Abs(hitCheck.knockback.x) > Mathf.Abs(hitCheck.knockbackSlowDown))
			hitCheck.knockback -= new Vector2(hitCheck.knockbackSlowDown, 0f);
		else
			hitCheck.knockback = Vector2.zero;
	}

	public void stopAttacking()
	{
		animator.SetBool("isAttacking", false);

		attackSpeedTimer = 0f;
	}

	public void stopDashing()
	{
		dashTimer = 0f;

		gameObject.layer = 8;
	}

	public void stopPushing()
	{
		animator.SetBool("isPushing", false);
	}

	public void stopThrowing()
	{
		animator.SetBool("isThrowing", false);

		attackSpeedTimer = 0f;
	}

	public Vector3 GetAbsolutePosition(GameObject go)
	{
		Vector3 position = Vector3.zero;

		Transform p = go.transform.parent;
		while (p != null)
		{
			position += p.position;
			p = p.transform.parent;
		}

		position += go.transform.position;

		return position;
	}

	public Quaternion GetAbsoluteRotation(GameObject go)
	{
		Quaternion rotation = Quaternion.identity;

		Transform p = go.transform.parent;
		while (p != null)
		{
			rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z + p.rotation.eulerAngles.z);
			p = p.transform.parent;
		}

		rotation = Quaternion.Euler(0f, 0f, rotation.eulerAngles.z + go.transform.rotation.eulerAngles.z);

		return rotation;
	}

	public void throwWeapon()
	{
		if (weaponPossession.weaponID > -1)
		{
			GameObject thrownObject = Instantiate(weaponPossession.gameObject, transform.position, Quaternion.Euler(0f,0f,0f));
			ThrownObject thrownObjectScript = thrownObject.AddComponent<ThrownObject>();

			thrownObjectScript.startPos = weaponPossession.transform.position;
			thrownObject.transform.localScale = new Vector2(2f, 2f);

			thrownObjectScript.breakableBreaksOnCollision = weaponPossession.weaponID == 0 ? false : true;

			Vector3 velocity = Vector3.zero;
			int dir = animator.GetInteger("direction");
			if (dir == 0)
			{
				velocity.x = 0;
				velocity.y = throwSpeed;
				thrownObject.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
			}
			else if (dir == 1)
			{
				velocity.x = throwSpeed;
				velocity.y = 0;
			}
			else if (dir == 2)
			{
				velocity.x = 0;
				velocity.y = -throwSpeed;
				thrownObject.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
			}
			else if (dir == 3)
			{
				velocity.x = -throwSpeed;
				velocity.y = 0;
			}
			thrownObjectScript.throwVelocity = velocity;
			thrownObjectScript.throwRotation = 0f;

			weaponPossession.weaponID = -1;
		}
	}
}
