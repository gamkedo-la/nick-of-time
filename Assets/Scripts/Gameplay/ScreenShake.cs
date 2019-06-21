using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
	//public float duration = 0.2f;
	//public float intensity = 0.5f;

	// temp: uncomment for quick testing by pressing 'p'
	//void Update ( )
	//{
	//	if ( Input.GetKeyDown( "p" ) )
	//	{
	//		StartCoroutine( Shake( duration, intensity ) );
	//	}
	//}

	public void SmallShake() { StartCoroutine(Shake(0.015f, 0.2f)); }

	public void MediumShake() { StartCoroutine(Shake(0.015f, 0.4f)); }

	public void BigShake() { StartCoroutine(Shake(0.015f, 1f)); }

	public void Earthquake() { StartCoroutine(Shake(5f, 0.25f)); }

	public IEnumerator Shake(float duration, float intensity)
	{
		Vector3 originalPos = transform.position;
		float elapsedTime = 0.0f;

		while (elapsedTime < duration)
		{
			float fade = Mathf.Max( 1 - ( elapsedTime / duration ), 0 );
			float x = Random.Range( -0.1f, 0.1f ) * intensity * fade;
			float y = Random.Range( -0.1f, 0.1f ) * intensity * fade;
			float z = 0.0f;
			Vector3 modVec = new Vector3( x, y, z );
			transform.position += modVec;

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.position = originalPos;
	}
}
