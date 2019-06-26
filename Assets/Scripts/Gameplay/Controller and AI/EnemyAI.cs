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

	private GameObject attackAreaObject;

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

		attackAreaObject = transform.GetChild(0).GetChild(1).GetChild(0).gameObject;

		actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
	}

	void Update ()
	{
		walkInput = Vector2.zero;

		if(targetObjects != null && animator.GetBool("isSpawned"))
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
			if(Vector2.Distance(
					new Vector2( targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y ),
					new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y )
					) < walkMaxDistance
				&& walkTimer <= 0f
			)
			{
				if(targetObjects[targetIndex].transform.position.x + 0.36f < rigidbody.transform.position.x)
					walkInput.x = -1f;
				if(targetObjects[targetIndex].transform.position.y + 0.04f < rigidbody.transform.position.y)
					walkInput.y = -1f;
				if(targetObjects[targetIndex].transform.position.x - 0.36f > rigidbody.transform.position.x)
					walkInput.x = 1f;
				if(targetObjects[targetIndex].transform.position.y - 0.04f > rigidbody.transform.position.y)
					walkInput.y = 1f;

				if(walkTimer <= -walkTime)
					walkTimer = Random.Range(walkMinDelay, walkMaxDelay);
			}

			if (walkInput != Vector2.zero)
			{
				if (animator.GetBool("isAttacking"))
					speed = attackSpeed;
				else
					speed = walkSpeed / (actionTimer > 0f ? noActionFactor : 1f);
			}

			if(actionTimer <= 0f)
			{
				//Determining whether to attack or not
				if(!animator.GetBool("isAttacking")
					&& targetObjects[targetIndex].GetComponent<HitCheck>().hp > 0f
					&& Vector2.Distance(
					new Vector2( targetObjects[targetIndex].transform.position.x, targetObjects[targetIndex].transform.position.y ),
					new Vector2( rigidbody.transform.position.x, rigidbody.transform.position.y )
					) <= attackMaxDistance)
				{
					animator.SetBool("isAttacking", true);
					
					if(aud != null && TogglesValues.sound)
						aud.PlayOneShot(attackSound);

					actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
				}
			}

			actionTimer -= Time.deltaTime;
			walkTimer -= Time.deltaTime;
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

	void OnCollisionEnter2D()
	{
		walkCollided = true;
	}

	public void completeSpawn()
	{
		animator.SetBool("isSpawned", true);

		gameObject.layer = LayerMask.NameToLayer("Enemy");
	}

	public void stopAttacking() {
		animator.SetBool("isAttacking", false);
		
		actionTimer = Random.Range(actionMinDelay, actionMaxDelay);
	}
}








