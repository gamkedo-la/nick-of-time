using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float walkSpeed = 1f;
	public float attackSpeed = 2f;
	public float dashSpeed = 3f;
	public float dashTime = 0.2f;

	[Space]
	public float regenerateActionPointsPerSec = 0.025f;

	[Space]
	public float attackActionDeplete = 0.4f;
	public float dashActionDeplete = 0.25f;

	[Space]
	public string walkVerticalInput = "Vertical";
	public string walkHorizontalInput = "Horizontal";
	public string attackInput = "Fire1";
	public string dashInput = "Jump";

	[Space]
	public AudioClip attackSound;
	public AudioClip dashSound;

	[HideInInspector] public float actionPoints = 1f;

	private float speed = 0f;
	private bool isDashing = false;
	private float dashTimer = 0f;

	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer sprRenderer;
	private HitCheck hitCheck;

	private AudioSource aud = null;

	//private GameObject shingObject;

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
			walkInput = new Vector2(Input.GetAxisRaw(walkHorizontalInput), Input.GetAxisRaw(walkVerticalInput));

			if (walkInput != Vector2.zero)
			{
				if (!animator.GetBool("isAttacking") && !isDashing)
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
					else if (walkInput.x != 0)
					{
						animator.SetInteger("direction", 1);
					}
					
					if (walkInput.x > 0) sprRenderer.flipX = true;
					else sprRenderer.flipX = false;
				}
			}
			else
			{
				speed = 0f;
				animator.SetBool("isWalking", false);
			}

			/*
			//Attack
			if (actionPoints >= attackActionDeplete
				&& !animator.GetBool("isAttacking")
				&& !isDashing
				&& Input.GetButtonDown(attackInput))
			{
				animator.SetBool("isAttacking", true);
				animator.SetBool("isWalking", false);
				speed = attackSpeed;

				
				shingObject.GetComponent<Animator>().SetBool("isAttacking", true);
				//shingObject.GetComponent<SpriteRenderer>().flipX = sprRenderer.flipX;

				if (!sprRenderer.flipX)
				{
					shingObject.transform.position = transform.position + new Vector3(0.16f, 0f, 0f);
					shingObject.transform.localScale = new Vector3(1f, 1f, 1f);
				}
				else
				{
					shingObject.transform.position = transform.position + new Vector3(-0.16f, 0f, 0f);
					shingObject.transform.localScale = new Vector3(-1f, 1f, 1f);
				}
				

				actionPoints -= attackActionDeplete;

				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(attackSound);
			}
			*/

			//Dash
			if (actionPoints >= dashActionDeplete
				&& !isDashing
				&& !animator.GetBool("isAttacking")
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

			dashTimer -= Time.deltaTime;          
			
		}
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
		}

		if (Mathf.Abs(hitCheck.knockback.x) > Mathf.Abs(hitCheck.knockbackSlowDown))
			hitCheck.knockback -= new Vector2(hitCheck.knockbackSlowDown, 0f);
		else
			hitCheck.knockback = Vector2.zero;
	}

	public void stopAttacking()
	{
		animator.SetBool("isAttacking", false);

		//shingObject.GetComponent<Animator>().SetBool("isAttacking", false);
	}

	public void stopDashing()
	{
		dashTimer = 0f;

		gameObject.layer = 8;
	}
}
