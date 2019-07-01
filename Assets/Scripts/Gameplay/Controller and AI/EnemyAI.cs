using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	[HideInInspector] public GameObject[] targetObjects = null;

	public enum EnemyAIType
	{
		Animated,
		Rotational,
		Procedural
	};

	public EnemyAIType type = EnemyAIType.Animated;
	public float damageToPlayer = 0.1f;

	[Space]
	public float walkSpeed = 1f;
	public float walkMaxDistance = 3.36f;
	public float walkTime = 0f;
	public float walkMinDelay = 0f;
	public float walkMaxDelay = 0f;

	[Space]
	public float attackSpeed = 0.5f;
	public float attackMaxDistance = 0.48f;

	[Space]
	public AudioClip attackSound;

	[Space]
	public float actionMinDelay = 1f;
	public float actionMaxDelay = 1.25f;
	public float noActionFactor = 1.4f;

	[Space]
	public Collider2D attackCollider;
	
	private float speed = 0f;

	private float walkTimer = 0f;
	private bool walkCollided = false;

	private float actionTimer = 0f;

	private Rigidbody2D rigidbody;
	private Animator animator;
	private SpriteRenderer sprRenderer;
	private HitCheck hitCheck;

	private AudioSource aud = null;
	
	private Vector2 walkInput = Vector2.zero;

	//For rotational and procedural AI types
	private bool isSpawned = false;
	[HideInInspector] public bool isAttacking = false;
	private float rotationSpeed = 720f;
	private bool rotationAlt = true;
	private bool rotationClockwise = false;
	private Vector3 targetPosition = Vector3.zero;
	private float angleAtTargetTime = 1.2f;
	private float angleAtTargetTimer = 0f;
	private TrailRenderer trail;

	void Start () {
		targetObjects = GameObject.FindGameObjectsWithTag("Player");

		rigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sprRenderer = GetComponent<SpriteRenderer>();
		hitCheck = GetComponent<HitCheck>();

		aud = GetComponent<AudioSource>();
		if(aud == null)
			aud = FindObjectOfType<AudioSource>();
			
		actionTimer = Random.Range(actionMinDelay, actionMaxDelay);

		angleAtTargetTimer = angleAtTargetTime;

		if(type == EnemyAIType.Rotational || type == EnemyAIType.Procedural)
			trail = transform.GetChild(3).gameObject.GetComponent<TrailRenderer>();
	}

	void Update ()
	{
		walkInput = Vector2.zero;

		if (targetObjects != null)
		{
			if (type == EnemyAIType.Animated && animator.GetBool("isSpawned"))
			{
				int targetIndex = -1;
				int indexLeft = -1;

				//Determining the nearest player to target on
				float smallestDistance = 1000f;
				float distance = smallestDistance;
				for (int i = 0; i < targetObjects.GetLength(0); i++)
				{
					if (targetObjects[i] != null)
					{
						distance = Vector3.Distance(rigidbody.transform.position, targetObjects[i].transform.position);
						indexLeft = i;
					}

					float prevSmallestDistance = smallestDistance;
					smallestDistance = Mathf.Min(distance, smallestDistance);

					if (prevSmallestDistance != smallestDistance) targetIndex = i;
				}

				if (indexLeft <= -1) return;

				if (targetIndex <= -1) targetIndex = indexLeft;

				//Determining walkInput
				if (Vector2.Distance(
						new Vector2(targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y),
						new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y)
						) < walkMaxDistance
					&& walkTimer <= 0f
				)
				{
					if (targetObjects[targetIndex].transform.position.x + 0.36f < rigidbody.transform.position.x)
						walkInput.x = -1f;
					if (targetObjects[targetIndex].transform.position.y + 0.04f < rigidbody.transform.position.y)
						walkInput.y = -1f;
					if (targetObjects[targetIndex].transform.position.x - 0.36f > rigidbody.transform.position.x)
						walkInput.x = 1f;
					if (targetObjects[targetIndex].transform.position.y - 0.04f > rigidbody.transform.position.y)
						walkInput.y = 1f;

					if (walkTimer <= -walkTime)
						walkTimer = Random.Range(walkMinDelay, walkMaxDelay);
				}

				if (walkInput != Vector2.zero)
				{
					if (animator.GetBool("isAttacking"))
						speed = attackSpeed;
					else
						speed = walkSpeed / (actionTimer > 0f ? noActionFactor : 1f);
				}

				if (actionTimer <= 0f)
				{
					//Determining whether to attack or not
					if (!animator.GetBool("isAttacking")
						&& targetObjects[targetIndex].GetComponent<HitCheck>().hp > 0f
						&& Vector2.Distance(
						new Vector2(targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y),
						new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y)
						) <= attackMaxDistance)
					{
						animator.SetBool("isAttacking", true);

						if (aud != null && TogglesValues.sound)
							aud.PlayOneShot(attackSound);

						actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
					}
				}

				actionTimer -= Time.deltaTime;
				walkTimer -= Time.deltaTime;
			}
			else if (type == EnemyAIType.Rotational)
			{
				int targetIndex = -1;
				int indexLeft = -1;

				//Determining the nearest player to target on
				float smallestDistance = 1000f;
				float distance = smallestDistance;
				for (int i = 0; i < targetObjects.GetLength(0); i++)
				{
					if (targetObjects[i] != null)
					{
						distance = Vector3.Distance(rigidbody.transform.position, targetObjects[i].transform.position);
						indexLeft = i;
					}

					float prevSmallestDistance = smallestDistance;
					smallestDistance = Mathf.Min(distance, smallestDistance);

					if (prevSmallestDistance != smallestDistance) targetIndex = i;
				}

				if (indexLeft <= -1) return;

				if (targetIndex <= -1) targetIndex = indexLeft;

				if (!isSpawned)
				{
					transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.32f, 0.32f, 0.32f), 0.1f);
					transform.localRotation = Quaternion.Euler(0f, 0f, transform.localRotation.eulerAngles.z + (rotationSpeed * Time.deltaTime));

					if (transform.localScale.x >= 0.315f && transform.localScale.y >= 0.315f)
					{
						transform.localScale = new Vector3(0.32f, 0.32f, 0.32f);
						isSpawned = true;
					}
				}
				else
				{
					//Determining walkInput
					if (Vector2.Distance(
							new Vector2(targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y),
							new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y)
							) < walkMaxDistance
						&& walkTimer <= 0f
					)
					{
						if (targetObjects[targetIndex].transform.position.x + 0.36f < rigidbody.transform.position.x)
							walkInput.x = -1f;
						if (targetObjects[targetIndex].transform.position.y + 0.04f < rigidbody.transform.position.y)
							walkInput.y = -1f;
						if (targetObjects[targetIndex].transform.position.x - 0.36f > rigidbody.transform.position.x)
							walkInput.x = 1f;
						if (targetObjects[targetIndex].transform.position.y - 0.04f > rigidbody.transform.position.y)
							walkInput.y = 1f;

						if (walkTimer <= -walkTime)
							walkTimer = Random.Range(walkMinDelay, walkMaxDelay);
					}

					if (actionTimer <= 0f
						&& targetObjects[targetIndex].GetComponent<HitCheck>().hp > 0f
						&& Vector2.Distance(
						new Vector2(targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y),
						new Vector2(rigidbody.transform.position.x, rigidbody.transform.position.y)
						) <= attackMaxDistance) isAttacking = true;

					if (isAttacking)
					{
						trail.Clear();
						trail.enabled = false;

						if (targetPosition != Vector3.zero)
						{
							attackCollider.enabled = true;
							transform.position = Vector3.MoveTowards(transform.position, targetPosition, attackSpeed * Time.deltaTime);

							if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
							{
								isAttacking = false;
								attackCollider.enabled = false;
								actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
								targetPosition = Vector3.zero;
								angleAtTargetTimer = angleAtTargetTime;
							}
						}
						else if (angleAtTargetTimer <= 0f)
						{
							GetComponent<BlinkEffect>().Unblink();

							if (targetPosition == Vector3.zero)
								targetPosition = targetObjects[targetIndex].transform.position;
						}
						else
						{
							float angle = transform.localRotation.eulerAngles.z;
							float followAngle = (Mathf.Atan2(
							targetObjects[targetIndex].transform.position.y - transform.position.y,
							targetObjects[targetIndex].transform.position.x - transform.position.x) * Mathf.Rad2Deg);

							followAngle += 270f;

							if (followAngle >= 360f)
								followAngle -= 360f;
								
							angle = Mathf.Lerp(angle, followAngle, 0.25f);
							transform.localRotation = Quaternion.Euler(0f, 0f, angle);

							if (Random.Range(0f, 1f) < 0.2f)
							{
								if (!GetComponent<BlinkEffect>().IsBlinking())
									GetComponent<BlinkEffect>().Blink();
								else
									GetComponent<BlinkEffect>().Unblink();
							}

							angleAtTargetTimer -= Time.deltaTime;
						}
					}
					else
					{
						if (rotationAlt)
						{
							trail.enabled = true;

							rotationSpeed = Mathf.Lerp(rotationSpeed, 1020f * (rotationClockwise ? -1f : 1f), 0.1f);

							if (Mathf.Abs(rotationSpeed) >= 1019.5f)
								rotationAlt = !rotationAlt;

							speed = walkSpeed;
						}
						else
						{
							trail.Clear();
							trail.enabled = false;

							rotationSpeed = Mathf.Lerp(rotationSpeed, 30f * (rotationClockwise ? -1f : 1f), 0.1f);

							if (Mathf.Abs(rotationSpeed) <= 30.1f)
							{
								rotationAlt = !rotationAlt;
								rotationClockwise = !rotationClockwise;
							}

							speed = walkSpeed / 2f;
						}

						transform.localRotation = Quaternion.Euler(0f, 0f, transform.localRotation.eulerAngles.z + (rotationSpeed * Time.deltaTime));
					}

					actionTimer -= Time.deltaTime;
				}
			}
		}
	}

	void FixedUpdate () {
		if(hitCheck.hp > 0f)
		{
			if(!walkCollided)
				rigidbody.MovePosition( new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y ) + (walkInput * speed * Time.deltaTime)
				+ (-hitCheck.knockback * Time.deltaTime));
		}
		else
		{
			rigidbody.MovePosition( new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y )
			+ (-hitCheck.knockback * Time.deltaTime));

			stopAttacking();
			gameObject.layer = LayerMask.NameToLayer("Dash");
		}

		if (Mathf.Abs(hitCheck.knockback.x) > Mathf.Abs(hitCheck.knockbackSlowDown)
		|| Mathf.Abs(hitCheck.knockback.y) > Mathf.Abs(hitCheck.knockbackSlowDown))
			hitCheck.knockback -= hitCheck.knockbackSlowDownVector;
		else
			hitCheck.knockback = Vector2.zero;

		walkCollided = false;
	}

	void OnCollisionEnter2D( Collision2D coll )
	{
		if (!coll.gameObject.name.Contains("Player") && coll.gameObject.layer != LayerMask.NameToLayer("Player"))
		{
			walkCollided = true;

			//for rotating and procedural AI
			if (isAttacking)
			{
				isAttacking = false;
				attackCollider.enabled = false;
				actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
				targetPosition = Vector3.zero;
				angleAtTargetTimer = angleAtTargetTime;
			}
		}
	}

	public void completeSpawn()
	{
		animator.SetBool("isSpawned", true);

		gameObject.layer = LayerMask.NameToLayer("Enemy");
	}

	public void stopAttacking() {
		if (animator)
			animator.SetBool("isAttacking", false);
		else
		{
			isAttacking = false;
			attackCollider.enabled = false;
			targetPosition = Vector3.zero;
			angleAtTargetTimer = angleAtTargetTime;
		}
		
		actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
	}
}








