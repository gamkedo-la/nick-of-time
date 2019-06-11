using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestAreaLighting : MonoBehaviour
{
	public CameraTriggerControl trigger;
	public GameObject lightingGrpsParent;
	public GameObject lightingGrp;
	public Camera jointCamera;

	[Space]
	public bool forceJoint = false;
	public bool findLightingGrp = false;
	public bool show = false;
	public bool reset = false;

	private Vector3 prevCamPos = Vector3.zero;
	private Vector3 prevMoodPos = Vector3.zero;
	
    void Update()
    {
		if (trigger != null)
		{
			if (findLightingGrp)
			{
				for (int i = 0; i < lightingGrpsParent.transform.childCount; i++)
				{
					if (lightingGrpsParent.transform.GetChild(i).gameObject.name == trigger.gameObject.name + "Grp")
					{
						lightingGrp = lightingGrpsParent.transform.GetChild(i).gameObject;
						break;
					}
				}

				findLightingGrp = false;
			}

			if (reset)
			{
				show = false;

				if (forceJoint && jointCamera != null)
				{
					jointCamera.transform.position = prevCamPos;
					jointCamera.orthographicSize = 1.4f;
				}
				else
				{
					trigger.cam.transform.position = prevCamPos;
					trigger.cam.orthographicSize = 1.4f;
				}

				trigger.moodAmbianceLerper.gameObject.GetComponent<Light>().color = Color.black;
				trigger.moodAmbianceLerper.gameObject.GetComponent<Light>().intensity = 0f;
				trigger.moodAmbianceLerper.gameObject.transform.position = prevMoodPos;

				prevCamPos = Vector3.zero;
				prevMoodPos = Vector3.zero;
				reset = false;

				lightingGrp.SetActive(false);
			}
			else if (show)
			{
				if (forceJoint && jointCamera != null)
				{
					if (prevCamPos == Vector3.zero)
						prevCamPos = jointCamera.transform.position;

					jointCamera.transform.position = new Vector3(trigger.gameObject.transform.position.x, trigger.gameObject.transform.position.y, jointCamera.gameObject.transform.position.z);
					jointCamera.orthographicSize = trigger.size;
				}
				else
				{
					if (prevCamPos == Vector3.zero)
						prevCamPos = trigger.cam.gameObject.transform.position;

					trigger.cam.transform.position = new Vector3(trigger.gameObject.transform.position.x, trigger.gameObject.transform.position.y, trigger.cam.gameObject.transform.position.z);
					trigger.cam.orthographicSize = trigger.size;
				}

				if (prevMoodPos == Vector3.zero)
					prevMoodPos = trigger.moodAmbianceLerper.gameObject.transform.position;

				trigger.moodAmbianceLerper.gameObject.GetComponent<Light>().color = trigger.ambientColor;
				trigger.moodAmbianceLerper.gameObject.GetComponent<Light>().intensity = trigger.ambientIntensity;
				trigger.moodAmbianceLerper.gameObject.transform.position = new Vector3(trigger.gameObject.transform.position.x, trigger.gameObject.transform.position.y, trigger.moodAmbianceLerper.gameObject.transform.position.z); ;

				lightingGrp.SetActive(true);
			}
		}
    }
}
