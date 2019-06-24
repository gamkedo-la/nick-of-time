using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerp : MonoBehaviour
{
	public bool swap = false;

	public bool blackAtStart = true;

	public Color color;
	public float intensity;
	public float factor;

	[HideInInspector] public bool disable = false;

	private Light l;

    void Start()
    {
		l = gameObject.GetComponent<Light>();

		if (blackAtStart)
		{
			l.color = Color.black;
			l.intensity = 0f;
		}

		disable = false;
    }

	void OnEnable()
	{
		l = gameObject.GetComponent<Light>();

		if (blackAtStart)
		{
			l.color = Color.black;
			l.intensity = 0f;
		}
		
		disable = false;
	}

	void Update()
    {
		if (disable)
		{
			Color lcolor = l.color;
			lcolor.r = Mathf.Lerp(lcolor.r, 0f, factor);
			lcolor.g = Mathf.Lerp(lcolor.g, 0f, factor);
			lcolor.b = Mathf.Lerp(lcolor.b, 0f, factor);
			l.color = lcolor;

			l.intensity = Mathf.Lerp(l.intensity, 0f, factor);
		}
		else
		{
			Color lcolor = l.color;
			lcolor.r = Mathf.Lerp(lcolor.r, color.r, factor);
			lcolor.g = Mathf.Lerp(lcolor.g, color.g, factor);
			lcolor.b = Mathf.Lerp(lcolor.b, color.b, factor);
			l.color = lcolor;

			l.intensity = Mathf.Lerp(l.intensity, intensity, factor);

			if (transform.parent != null && transform.parent.GetComponent<DisableAfterDelay>() != null)
				disable = true;
		}
	}

	private void OnDrawGizmos()
	{
		if (swap)
		{
			l = gameObject.GetComponent<Light>();

			Color tempColor = color;
			color = l.color;
			l.color = tempColor;

			float tempIntensity = intensity;
			intensity = l.intensity;
			l.intensity = tempIntensity;

			swap = false;
		}
	}
	
}
