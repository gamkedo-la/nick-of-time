using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffectLerper : MonoBehaviour
{
	public int effectIndex = 0;
	public int state = 0;
	public float value = 0f;

	public void ResetEffect()
	{
		ImageEffect.SetImageEffectMaterialIndex(0);
		ImageEffect.SetImageEffectValue(0.04f);
		enabled = false;
	}

	void Start()
    {
        
    }
	
    void Update()
    {
		value += state * Time.deltaTime;

		if (value <= 0f) value = 0f;
		else if (value >= 1f) value = 1f;

		ImageEffect.SetImageEffectMaterialIndex(effectIndex);
		ImageEffect.SetImageEffectValue(value);
    }
}
