using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ObjectShake : MonoBehaviour
{

    private RectTransform rectTransform;

    [SerializeField]
    private GameObject objectToShake;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public IEnumerator Shake(float magnitude, float duration)
    {       
            
            Vector2 originalPosition = objectToShake.transform.localPosition;

            float elapsed = 0.0f;


            while (elapsed < duration)
            {
                float x = Random.Range(-1f, 1f) * magnitude;
                float y = Random.Range(-1f, 1f) * magnitude;

                objectToShake.transform.localPosition = new Vector2(x, y);

                // transform.localPosition = new Vector2(x, y);

                elapsed += Time.deltaTime;

                yield return null;
            }

            objectToShake.transform.localPosition = originalPosition;
    }
}



