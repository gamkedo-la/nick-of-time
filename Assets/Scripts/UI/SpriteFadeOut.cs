﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFadeOut : MonoBehaviour
{
	public float delay = 0.1f;
	public bool destroy = true;
	public bool disable = false;

	private float timer = 0f;

	private SpriteRenderer sprRend;

    void Start()
    {
		sprRend = gameObject.GetComponent<SpriteRenderer>();

		timer = delay;
    }
	
    void Update()
    {
		if (timer <= 0f)
		{
			if (destroy)
			{
				Destroy(gameObject);
			}
			else if (disable)
			{
				gameObject.SetActive(false);
				Destroy(this);
			}
			else
			{
				timer = delay;
			}
		}
		else
		{
			Color col = sprRend.color;
			col.a = timer / delay;
			sprRend.color = col;
			
			timer -= Time.deltaTime;
		}
    }
}
