using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesParticle : MonoBehaviour
{
	public float minDestroyDelay = 10f;
	public float maxDestroyDelay = 20f;
	public float physicsDelay = 0.5f;
	public bool doDestroy = true;
	public bool containBreakable = false;

	public Vector2 minVelocity = new Vector2(-1f, -0.1f);
	public Vector2 maxVelocity = new Vector2(1f, -1f);

	private float destroyDelay;

	private BoxCollider2D collider;
	private Rigidbody2D rigidbody;

	private bool done = false;
	private bool triggered = false;

	void Setup()
	{
		destroyDelay = Random.Range(minDestroyDelay, maxDestroyDelay);

		rigidbody.velocity = new Vector2(
			Random.Range(minVelocity.x, maxVelocity.x),
			Random.Range(minVelocity.y, maxVelocity.y));

		done = false;
	}

	private void Start()
	{
		collider = GetComponent<BoxCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();

		Setup();
	}

	private void OnEnable()
	{
		collider = GetComponent<BoxCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();

		Setup();
	}

	void Update()
	{
		if (!done)
		{
			if (physicsDelay <= 0f)
			{
				//collider.isTrigger = true;
				rigidbody.gravityScale = 0f;
				rigidbody.velocity = Vector2.zero;
				rigidbody.angularVelocity = 0f;
			}

			if (destroyDelay <= 0f)
			{
				if (doDestroy)
					Destroy(gameObject);
				else
				{
					done = true;

					if (containBreakable)
						gameObject.GetComponent<Breakable>().enabled = true;
				}
			}

			physicsDelay -= Time.deltaTime;
			destroyDelay -= Time.deltaTime;
		}
		else if (!triggered)
		{
			rigidbody.velocity = Vector2.zero;
			rigidbody.angularVelocity = 0f;
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (done)
		{
			triggered = true;
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (done)
		{
			triggered = false;
		}
	}
}
