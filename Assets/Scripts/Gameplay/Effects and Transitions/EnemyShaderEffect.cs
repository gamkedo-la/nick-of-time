using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShaderEffect : MonoBehaviour
{
	[SerializeField] private float outlineLerpFactor = 0.05f;
	[SerializeField] private float hitLerpFactor = 0.15f;

	private float outlineEffect = 0f;
	private float hitEffect = 0f;

	private Material mat;

	private float timer = 0f;
	
    void Update()
    {
		mat = gameObject.GetComponent<SpriteRenderer>().material;

		outlineEffect = Mathf.Lerp(outlineEffect, 0.5001f, outlineLerpFactor);
		hitEffect = Mathf.Lerp(hitEffect, 0f, hitLerpFactor);

		mat.SetFloat("outlineEffect", outlineEffect);
		mat.SetFloat("hitEffect", hitEffect);
	}

	public void HitEffect()
	{
		hitEffect = 0.5f;
	}
}
