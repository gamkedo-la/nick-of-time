using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopySpriteRendererComponent : MonoBehaviour
{
	public SpriteRenderer sprRendererExternal;

	private SpriteRenderer sprRenderer;

	void Start()
    {
		sprRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
		sprRenderer.sprite = sprRendererExternal.sprite;
		sprRenderer.flipX = sprRendererExternal.flipX;
    }
}
