using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	public void SmallShake() { StartCoroutine(Shake(0.015f, 0.2f)); }

	public void MediumShake() { StartCoroutine(Shake(0.015f, 0.4f)); }

	public void BigShake() { StartCoroutine(Shake(0.015f, 1f)); }

	public void Earthquake() { StartCoroutine(Shake(5f, 0.25f)); }

	public IEnumerator Shake(float duration, float intensity)
	{
		float elapsedTime = 0.0f;

		while (elapsedTime < duration)
		{
			// Fade makes the shake effect less intense as it goes
			float fade = Mathf.Max( 1 - ( elapsedTime / duration ), 0 );
			float x = Random.Range( -0.1f, 0.1f ) * intensity * fade;
			float y = Random.Range( -0.1f, 0.1f ) * intensity * fade;
			float z = 0.0f;
			Vector3 modVec = new Vector3( x, y, z );
			transform.position += modVec;

			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}
}
