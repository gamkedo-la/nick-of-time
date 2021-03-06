﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicDestroyFunction : MonoBehaviour {

	public void destroy()
	{
		Destroy(gameObject);
	}

	public void destroyParent()
	{
		Destroy(gameObject.transform.parent.gameObject);
	}
}
