using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ObjectShake : MonoBehaviour
{
	public bool keepShaking = false;
	public float delay = 0.01f;
	public float intensity = 1f;

	private float timer = 0f;
	private Vector2 originalPosition;

	private void Awake()
	{
		UpdateFixedPosition();
	}

	public void Update()
	{
		if (keepShaking)
		{
			if (timer <= 0f)
			{
				transform.localPosition = originalPosition;
				Shake(intensity);

				timer = delay;
			}

			timer -= Time.deltaTime;
		}
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
			Shake(magnitude);

			elapsed += Time.deltaTime;

			yield return null;
		}

		transform.localPosition = originalPosition;
	}

	private void Shake(float magnitude)
	{
		float x = Random.Range(-1f, 1f) * magnitude;
		float y = Random.Range(-1f, 1f) * magnitude;

		transform.localPosition = new Vector2(x, y);
	}
}



