﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushedObject : MonoBehaviour
{
	private float minVelocity = 1f;
	private Rigidbody2D rb;

    void Start()
    {
		rb = GetComponent<Rigidbody2D>();

		if (rb == null)
			Destroy(this);
    }
	
    void Update()
    {
		if (rb.velocity == Vector2.zero)
			Destroy(this);
	}

	/*
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (rb != null && collision.gameObject.tag != "Player" && collision.gameObject.layer != LayerMask.NameToLayer("PiecesCollision")
		&& (minVelocity <= Mathf.Abs(rb.velocity.x) || minVelocity <= Mathf.Abs(rb.velocity.y)))
		{
			Breakable b1 = GetComponent<Breakable>();
			Breakable b2 = collision.gameObject.GetComponent<Breakable>();

			if (b2 != null)
			{
				b2.BreakObject();
			}

			if (b1 != null)
			{
				b1.BreakObject();
			}

			Destroy(this);
		}
	}
	*/
}
