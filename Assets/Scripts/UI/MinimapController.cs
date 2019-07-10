using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
	public Camera minimapCam;
	public bool focus = false;
	public bool noLerp = false;

	static public MinimapController[] instances = { null, null, null };

	private Vector3 prevLocalPosition;
	private Vector3 prevLocalScale;
	private float prevOrthoSize;
	private SpriteRenderer minimapBG;

	private static bool _focus = false;
	private bool changed = false;

    void Start()
    {
		prevLocalPosition = transform.localPosition;
		prevLocalScale = transform.localScale;
		prevOrthoSize = minimapCam.orthographicSize;
		minimapBG = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

		if (!instances[0]) instances[0] = this;
		else if (!instances[1]) instances[1] = this;
		else if (!instances[2]) instances[2] = this;
    }

	private void Update()
	{
		if (changed)
		{
			focus = _focus;
			changed = false;
		}
		/*
		if (Input.GetKeyDown(KeyCode.M))
		{
			_focus = !_focus;
			changed = true;
		}
		*/

		if (!noLerp)
		{
			if (focus)
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0f, 160f, 0f), 0.1f);
				transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(2f, 2f, 2f), 0.1f);
				minimapCam.orthographicSize = Mathf.Lerp(minimapCam.orthographicSize, 13f, 0.1f);
				minimapBG.color = Color.Lerp(minimapBG.color, new Color(0f, 0f, 0f, 0.25f), 0.1f);
			}
			else
			{
				transform.localPosition = Vector3.Lerp(transform.localPosition, prevLocalPosition, 0.1f);
				transform.localScale = Vector3.Lerp(transform.localScale, prevLocalScale, 0.1f);
				minimapCam.orthographicSize = Mathf.Lerp(minimapCam.orthographicSize, prevOrthoSize, 0.1f);
				minimapBG.color = Color.Lerp(minimapBG.color, new Color(0f, 0f, 0f, 0.05f), 0.1f);
			}
		}
	}
}
