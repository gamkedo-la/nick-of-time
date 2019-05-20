
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {
	
	public float walkSpeed = 1f;
	public float attackSpeed = 2f;
	public float rollSpeed = 3f;
	public float rollBoostFactor = 1f;
	
	public float regenerateActionPointsPerSec = 0.025f;
	
	public float attackActionDeplete = 0.4f;
	public float rollActionDeplete = 0.25f;
	
	public string walkVerticalInput = "Vertical";
	public string walkHorizontalInput = "Horizontal";
	public string attackInput = "Fire1";
	public string rollInput = "Jump";
	
	public AudioClip attackSound;
	public AudioClip rollSound;
	
	[HideInInspector] public float actionPoints = 1f;
	
	private float speed = 0f;
	
	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer sprRenderer;
	private HitCheck hitCheck;
	
	private AudioSource aud = null;
	
	private GameObject shingObject;
	
	private Vector2 walkInput = Vector2.zero;
	
	void Start () {
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprRenderer = GetComponent<SpriteRenderer>();
		hitCheck = GetComponent<HitCheck>();
		
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
		
		shingObject = transform.GetChild(1).GetChild(0).gameObject;
	}
	
	void Update() {
		if(Time.timeScale > 0f)
		{
		
		walkInput = new Vector2( Input.GetAxisRaw(walkHorizontalInput), Input.GetAxisRaw(walkVerticalInput) );
		
		if(walkInput != Vector2.zero)
		{
			if(!animator.GetBool("isAttacking") )
			{
				if(walkInput.x < 0) sprRenderer.flipX = true;
				else sprRenderer.flipX = false;

				if(!animator.GetBool("isRolling"))
				{
					animator.SetBool("isWalking", true);
					speed = walkSpeed;
				}
			}
		}
		else
		{
			animator.SetBool("isWalking", false);
		}
		
		//Attack
		if(actionPoints >= attackActionDeplete
			&&!animator.GetBool("isAttacking")
			&& !animator.GetBool("isRolling")
			&& Input.GetButtonDown(attackInput))
		{
			animator.SetBool("isAttacking", true);
			animator.SetBool("isWalking", false);
			speed = attackSpeed;
		
			shingObject.GetComponent<Animator>().SetBool("isAttacking", true);
			//shingObject.GetComponent<SpriteRenderer>().flipX = sprRenderer.flipX;
		
			if(!sprRenderer.flipX)
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
			
			if(aud != null && TogglesValues.sound)
				aud.PlayOneShot(attackSound);
		}
		
		//Roll
		if(actionPoints >= rollActionDeplete
			&& !animator.GetBool("isRolling")
			&& !animator.GetBool("isAttacking")
			&& Input.GetButtonDown(rollInput)
			&& hitCheck.knockback == Vector2.zero)
		{
			animator.SetBool("isRolling", true);
			animator.SetBool("isWalking", false);
			//speed = rollSpeed;
			gameObject.layer = 10;
			
			actionPoints -= rollActionDeplete;
			
			if(aud != null && TogglesValues.sound)
				aud.PlayOneShot(rollSound);
		}
		
		if(animator.GetBool("isAttacking")
			|| animator.GetBool("isRolling"))
		{
			if(walkInput == Vector2.zero)
			{
				if(sprRenderer.flipX)
				{
					walkInput.x = -1f;
				}
				else
				{
					walkInput.x = 1f;
				}
			}
		}
		
		if(actionPoints < 1f)
		{
			actionPoints += regenerateActionPointsPerSec * Time.deltaTime;
		}
		
		if(animator.GetBool("isRolling")
			&& speed < rollSpeed)
		{
			speed += Time.deltaTime * rollBoostFactor;
		}
		
		}
	}
	
	void FixedUpdate () {
		if(hitCheck.hp > 0f)
		{
			rigidbody.MovePosition( new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y ) + (walkInput * speed * Time.deltaTime) + (hitCheck.knockback * Time.deltaTime) );
		}
		else
		{
			rigidbody.MovePosition( new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y ) + (hitCheck.knockback * Time.deltaTime) );
			
			stopAttacking();
			stopRolling();
			gameObject.layer = 10;
		}
		
		if(Mathf.Abs(hitCheck.knockback.x) > Mathf.Abs(hitCheck.knockbackSlowDown))
			hitCheck.knockback -= new Vector2( hitCheck.knockbackSlowDown, 0f );
		else
			hitCheck.knockback = Vector2.zero;
	}
	
	public void stopAttacking() {
		animator.SetBool("isAttacking", false);
		
		shingObject.GetComponent<Animator>().SetBool("isAttacking", false);
	}
	
	public void stopRolling() {
		animator.SetBool("isRolling", false);
		
		gameObject.layer = 8;
	}
}
