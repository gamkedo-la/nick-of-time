using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectShake : MonoBehaviour
{
	private RectTransform rectTransform;
	private Vector2 originalPosition;
	
	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		UpdateFixedPosition();
	}

	public void UpdateFixedPosition()
	{
		originalPosition = transform.localPosition;
	}

	public IEnumerator Shake(float magnitude, float duration)
	{
		float elapsed = 0.0f;

		while (elapsed < duration)
		{
			float x = Random.Range(-1f, 1f) * magnitude;
			float y = Random.Range(-1f, 1f) * magnitude;

			transform.localPosition = new Vector2(x, y);

			elapsed += Time.deltaTime;

			yield return null;
		}

		transform.localPosition = originalPosition;
	}
}



