using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointCameraToPlayerCameras : MonoBehaviour
{
	private Camera[] cameras = new Camera[3];

    void Start()
    {
		cameras[0] = gameObject.transform.GetChild(0).gameObject.GetComponent<Camera>();
		cameras[1] = gameObject.transform.GetChild(1).gameObject.GetComponent<Camera>();
		cameras[2] = gameObject.transform.GetChild(2).gameObject.GetComponent<Camera>();
	}
	
    void Update()
    {
		cameras[0].GetComponent<LerpToTransform>().tr = cameras[1].GetComponent<LerpToTransform>().tr;
		cameras[0].GetComponent<LerpToCamSize>().size = cameras[1].GetComponent<LerpToCamSize>().size;

		cameras[0].gameObject.transform.position = cameras[1].gameObject.transform.position;
		cameras[0].orthographicSize = cameras[1].orthographicSize;

		cameras[0].GetComponent<ImageEffect>().materialIndex = cameras[1].GetComponent<ImageEffect>().materialIndex;
		cameras[0].GetComponent<ImageEffect>().effectIteration = cameras[1].GetComponent<ImageEffect>().effectIteration;
		cameras[0].GetComponent<ImageEffect>().value = cameras[1].GetComponent<ImageEffect>().value;
	}
}
