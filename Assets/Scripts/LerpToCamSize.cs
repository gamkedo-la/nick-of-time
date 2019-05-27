using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToCamSize : MonoBehaviour
{
	public float size;
	public float factor;

	private Camera cam;

    void Start()
    {
		cam = gameObject.GetComponent<Camera>();
		size = cam.orthographicSize;
    }
	
    void Update()
    {
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, size, factor);
    }
}
