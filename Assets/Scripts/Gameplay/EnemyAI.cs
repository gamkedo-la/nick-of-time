using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {
	
	public GameObject[] targetObjects = null;
	
	public float walkSpeed = 1f;
	public float walkMaxDistance = 3.36f;
	public float walkTime = 0f;
	public float walkMinDelay = 0f;
	public float walkMaxDelay = 0f;
	
	public float attackSpeed = 2f;
	public float attackMaxDistance = 0.48f;
	
	public float rollSpeed = 3f;
	public float rollTendency = 0.05f;
	public float rollMaxDistance = 1.12f;
	
	public AudioClip attackSound;
	public AudioClip rollSound;
	
	public float actionMinDelay = 1f;
	public float actionMaxDelay = 1.25f;
	
	private float speed = 0f;
	
	private float walkTimer = 0f;
	private float walkRandomizer = 0f;
	private bool walkCollided = false;
	
	private float actionTimer = 0f;
	
	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer sprRenderer;
	private HitCheck hitCheck;
	
	private AudioSource aud = null;
	
	private GameObject shingObject;
	
	private Vector2 walkInput = Vector2.zero;

	void Start () {
		targetObjects = GameObject.FindGameObjectsWithTag("Player");
		
		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprRenderer = GetComponent<SpriteRenderer>();
		hitCheck = GetComponent<HitCheck>();
		
		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
		
		shingObject = transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
		
		actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
	}
	
	void Update () {
		
		walkInput = Vector2.zero;
		
		if(targetObjects != null)
		{
			int targetIndex = -1;
			int indexLeft = -1;
		
			//Determining the nearest player to target on
			float smallestDistance = 1000f;
			float distance = smallestDistance;
			for(int i = 0; i < targetObjects.GetLength(0); i++)
			{
				if(targetObjects[i] != null)
				{
					distance = Vector3.Distance(rigidbody.transform.position, targetObjects[i].transform.position);
					indexLeft = i;
				}
				
				float prevSmallestDistance = smallestDistance;
				smallestDistance = Mathf.Min(distance, smallestDistance);
				
				if(prevSmallestDistance != smallestDistance) targetIndex = i;
			}
			
			if(indexLeft <= -1) return;
			
			if(targetIndex <= -1) targetIndex = indexLeft;
			
			//Determining walkInput
			if(!animator.GetBool("isAttacking")
				&& !animator.GetBool("isRolling")
				&& Vector2.Distance(
					new Vector2( targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y ),
					new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y )
					) < walkMaxDistance
				&& walkTimer <= 0f 
			)
			{
				if(targetObjects[targetIndex].transform.position.x + 0.36f < rigidbody.transform.position.x)
				{
					walkInput.x = -1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = true;
				}
				if(targetObjects[targetIndex].transform.position.y + 0.04f < rigidbody.transform.position.y)
				{
					walkInput.y = -1f;
				}
				if(targetObjects[targetIndex].transform.position.x - 0.36f > rigidbody.transform.position.x)
				{
					walkInput.x = 1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = false;
				}
				if(targetObjects[targetIndex].transform.position.y - 0.04f > rigidbody.transform.position.y)
				{
					walkInput.y = 1f;
				}
				
				if(walkTimer <= -walkTime)
				{
					walkTimer = Random.Range(walkMinDelay, walkMaxDelay);
				}
			}
			//Random walk when player is not under the minimal walk distance
			/*else if(walkTimer <= 0f)
			{
				if(walkRandomizer < 1f)
				{
					walkInput.x = -1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = true;
				}
				else if(walkRandomizer < 2f)
				{
					walkInput.y = -1f;
				}
				else if(walkRandomizer < 3f)
				{
					walkInput.x = 1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = false;
				}
				else if(walkRandomizer < 4f)
				{
					walkInput.y = 1f;
				}
				if(walkRandomizer < 5f)
				{
					walkInput.x = -1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = true;
					walkInput.y = -1f;
				}
				else if(walkRandomizer < 6f)
				{
					walkInput.x = 1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = false;
					walkInput.y = -1f;
				}
				else if(walkRandomizer < 7f)
				{
					walkInput.x = 1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = false;
					walkInput.y = 1f;
				}
				else if(walkRandomizer < 8f)
				{
					walkInput.x = -1f;
					if(!animator.GetBool("isAttacking") )
						sprRenderer.flipX = true;
					walkInput.y = 1f;
				}
				
				if(walkTimer <= -walkTime) 
				{
					walkTimer = Random.Range(walkMinDelay, walkMaxDelay);
					
					if(walkCollided)
					{
						if(walkRandomizer >= 7f)
							walkRandomizer -= 1f;
						else if(walkRandomizer <= 1f)
							walkRandomizer += 1f;
						else
							walkRandomizer += Random.Range(0f, 2f) - 1f;
						
						walkCollided = false;
					}
					else
					{
						walkRandomizer = Random.Range(0f, 8f);
					}
				}
			}*/
		
			if(walkInput != Vector2.zero)
			{
				//if(!animator.GetBool("isRolling"))
				//{
					animator.SetBool("isWalking", true);
					speed = walkSpeed;
				//}
			}
			else
			{
				animator.SetBool("isWalking", false);
			}
		
			if(actionTimer <= 0f)
			{
				//Determining whether to attack or not
				if(!animator.GetBool("isAttacking")
					&& !animator.GetBool("isRolling")
					&& targetObjects[targetIndex].GetComponent<HitCheck>().hp > 0f					
					&& Vector2.Distance(
					new Vector2( targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y ),
					new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y )
					) <= attackMaxDistance)
				{
					animator.SetBool("isAttacking", true);
					animator.SetBool("isWalking", false);
					speed = attackSpeed;
			
					shingObject.GetComponent<Animator>().SetBool("isAttacking", true);
					shingObject.GetComponent<SpriteRenderer>().flipX = sprRenderer.flipX;
			
					if(!sprRenderer.flipX)
					{
							shingObject.transform.position = transform.position + new Vector3(0.32f, 0f, 0f);
					}
					else
					{
							shingObject.transform.position = transform.position + new Vector3(-0.32f, 0f, 0f);
					}
					
					if(aud != null && TogglesValues.sound)
						aud.PlayOneShot(attackSound);
				}
			
				//Determining whether to roll or not
				if(!animator.GetBool("isRolling")
					&& !animator.GetBool("isAttacking")
					&& Random.Range(0f, 1f) <= rollTendency
					&& Vector2.Distance(
					new Vector2( targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y ),
					new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y )
					) <= rollMaxDistance
					&& hitCheck.knockback == Vector2.zero)
				{
					animator.SetBool("isRolling", true);
					animator.SetBool("isWalking", false);
					speed = rollSpeed;
					gameObject.layer = 10;
					
					if(aud != null && TogglesValues.sound)
						aud.PlayOneShot(rollSound);
				}
			}
		
			if(animator.GetBool("isAttacking") || animator.GetBool("isRolling"))
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
		
			actionTimer -= Time.deltaTime;
			walkTimer -= Time.deltaTime;
		
		}
	}
	
	void FixedUpdate () {
		if(hitCheck.hp > 0f)
		{
			rigidbody.MovePosition( new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y ) + (walkInput * speed * Time.deltaTime) + ((sprRenderer.flipX == true ? 1f : -1f) * hitCheck.knockback * Time.deltaTime));
		}
		else
		{
			rigidbody.MovePosition( new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y ) + ((sprRenderer.flipX == true ? 1f : -1f) * hitCheck.knockback * Time.deltaTime));
			
			stopAttacking();
			stopRolling();
			gameObject.layer = 10;
		}
		
		if(Mathf.Abs(hitCheck.knockback.x) > Mathf.Abs(hitCheck.knockbackSlowDown))
			hitCheck.knockback -= new Vector2( hitCheck.knockbackSlowDown, 0f );
		else
			hitCheck.knockback = Vector2.zero;
	}
	
	void OnCollisionEnter2D()
	{
		walkCollided = true;
	}
	
	public void stopAttacking() {
		animator.SetBool("isAttacking", false);
		
		shingObject.GetComponent<Animator>().SetBool("isAttacking", false);
		
		actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
	}
	
	public void stopRolling() {
		animator.SetBool("isRolling", false);
		
		actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
		
		gameObject.layer = 9;
	}
}








