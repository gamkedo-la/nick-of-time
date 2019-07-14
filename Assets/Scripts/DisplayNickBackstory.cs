using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayNickBackstory : MonoBehaviour
{
	[Multiline]
	public string story = "";

    void Start()
    {
        
    }
	
    void Update()
    {
		if (!GetComponent<Breakable>().enabled)
		{
			if (ActivateSingleMode.currentInstance != null)
			{
				ActivateSingleMode.currentInstance.paperViewObject.SetActive(true);
				ActivateSingleMode.currentInstance.paused = true;
				ActivateSingleMode.currentInstance.duelCamToSingleCam();
				ActivateSingleMode.currentInstance.paperViewObject.transform.GetChild(0).GetChild(2).
					GetComponent<TextMeshProUGUI>().text = story;
			}
			Time.timeScale = 0f;
			Destroy(this);
		}
    }
}
