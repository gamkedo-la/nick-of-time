using System.Collections;
using UnityEngine;


public class ObjectShake : MonoBehaviour
{
    public IEnumerator Shake(float magnitude, float duration)
    {
        Vector2 originalPosition = transform.localPosition;

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
