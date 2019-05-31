using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesParticle : MonoBehaviour {
	
	public float minDestroyDelay = 10f;
	public float maxDestroyDelay = 20f;
	public float physicsDelay = 0.5f;
	public bool doDestroy = true;
	
	public Vector2 minVelocity = new Vector2(-1f, -0.1f);
	public Vector2 maxVelocity = new Vector2(1f, -1f);
	
	private float destroyDelay;
	
	private BoxCollider2D collider;
	private Rigidbody2D rigidbody;

	void Setup ()
	{
		destroyDelay = Random.Range(minDestroyDelay, maxDestroyDelay);
		
		rigidbody.velocity = new Vector2(
			Random.Range(minVelocity.x, maxVelocity.x),
			Random.Range(minVelocity.y, maxVelocity.y));
	}

	private void Start()
	{
		collider = GetComponent<BoxCollider2D>();
		rigidbody = GetComponent<Rigidbody2D>();

		Setup();
	}
	
	void Update ()
	{
		if(physicsDelay <= 0f)
		{
			collider.enabled = false;
			rigidbody.gravityScale = 0f;
			rigidbody.velocity = Vector2.zero;
			rigidbody.angularVelocity = 0f;
		}
		
		if(destroyDelay <= 0f)
		{
			if (doDestroy)
			{
				Destroy(gameObject);
			}
			else
			{
				Setup();
				enabled = false;
			}
		}
		
		physicsDelay -= Time.deltaTime;
		destroyDelay -= Time.deltaTime;
	}
}
