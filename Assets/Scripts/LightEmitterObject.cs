using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class LightEmitterObject : MonoBehaviour
{
	public Color highLightColor;
	public float highLightIntensity;
	public float highLightRange;
	public float highLightSpotAngle;

	[Space]
	public Color midLightColor;
	public float midLightIntensity;
	public float midLightRange;
	public float midLightSpotAngle;

	[Space]
	public Color lowLightColor;
	public Color holderLightColor;

	private Material mat;

	private Light denseLight;
	private Light spreadLight;

	[HideInInspector] public bool disable = false;

    void Start()
    {
		mat = gameObject.GetComponent<SpriteRenderer>().sharedMaterial;
		denseLight = gameObject.transform.GetChild(0).GetChild(0).GetComponent<Light>();
		spreadLight = gameObject.transform.GetChild(0).GetChild(1).GetComponent<Light>();
		disable = false;
	}

	private void OnEnable()
	{
		Start();
	}

	private void Update()
	{
		Display();
	}
	
	void Display()
    {
		if (!mat)
			Start();

		mat.SetColor("highLightColor", highLightColor);
		mat.SetColor("midLightColor", midLightColor);
		mat.SetColor("lowLightColor", lowLightColor);
		mat.SetColor("holderLightColor", holderLightColor);

		denseLight.color = highLightColor;
		spreadLight.color = midLightColor;

		denseLight.range = highLightRange;
		spreadLight.range = midLightRange;

		denseLight.spotAngle = highLightSpotAngle;
		spreadLight.spotAngle = midLightSpotAngle;

		if (disable)
		{
			denseLight.intensity = Mathf.Lerp(denseLight.intensity, 0f, 0.15f);
			spreadLight.intensity = Mathf.Lerp(spreadLight.intensity, 0f, 0.15f);
		}
		else
		{
			denseLight.intensity = Mathf.Lerp(denseLight.intensity, highLightIntensity, 0.3f);
			spreadLight.intensity = Mathf.Lerp(spreadLight.intensity, midLightIntensity, 0.3f);
		}
	}
}
