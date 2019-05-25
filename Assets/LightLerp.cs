using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLerp : MonoBehaviour
{
	public Color color;
	public float intensity;
	public float factor;

	private Light l;

    void Start()
    {
		l = gameObject.GetComponent<Light>();

		color = l.color;
		intensity = l.intensity;
    }
	
    void Update()
    {
		Color lcolor = l.color;
		lcolor.r = Mathf.Lerp(lcolor.r, color.r, factor);
		lcolor.g = Mathf.Lerp(lcolor.g, color.g, factor);
		lcolor.b = Mathf.Lerp(lcolor.b, color.b, factor);
		l.color = lcolor;

		l.intensity = Mathf.Lerp(l.intensity, intensity, factor);
	}
}
