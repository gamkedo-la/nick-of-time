using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImageEffect : MonoBehaviour
{
	public int materialIndex = 0;
	public Material[] imageEffectMaterial;

	public int effectIteration = 0;

	[Range(0f, 1f)]
	public float value = 0;
	
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		imageEffectMaterial[materialIndex].SetFloat("value", value);

		RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height);
		Graphics.Blit(source, rt);

		for (int i = 0; i < effectIteration; i++)
		{
			RenderTexture rt2 = RenderTexture.GetTemporary(source.width, source.height);
			Graphics.Blit(rt, rt2, imageEffectMaterial[materialIndex]);
			RenderTexture.ReleaseTemporary(rt);
			rt = rt2;
		}

		Graphics.Blit(rt, destination);
		RenderTexture.ReleaseTemporary(rt);
	}
}
