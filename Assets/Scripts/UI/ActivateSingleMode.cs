using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSingleMode : MonoBehaviour {
	
	public Camera jointCam;
	
	public Camera cam1;
	public Camera cam2;
	
	public GameObject pauseObject;
	public string pauseInput = "Cancel";
	
	public GameObject dialogueBoxObject;
	
	public GameObject loseObject;
	
	public GameObject winObject;
	
	public GameObject p1WinObject;
	public GameObject p2WinObject;
	
	[HideInInspector] public bool paused = false;
	
	void singleCamToDuelCam()
	{
		jointCam.enabled = false;
			
		cam1.enabled = true;
		cam2.enabled = true;
			
	}
	
	void duelCamToSingleCam(bool otherCam = false)
	{
		jointCam.enabled = true;
			
		if(otherCam)
			jointCam.gameObject.GetComponent<LerpToTransform>().tr = cam2.gameObject.GetComponent<LerpToTransform>().tr;
		else
			jointCam.gameObject.GetComponent<LerpToTransform>().tr = cam1.gameObject.GetComponent<LerpToTransform>().tr;
			
		cam1.enabled = false;
		cam2.enabled = false;
	}

	void Start () {
		if (cam1 != null && cam2 != null)
			singleCamToDuelCam();
		else
		{
			jointCam.enabled = true;
		}
		
		//pauseObject.SetActive(false);
	}
	
	void Update ()
	{
		GameObject e = GameObject.FindWithTag("Enemy");

		//Exciting music when enemies appear!
		if (e == null)
		{
			if (MusicManager.instance != null)
				MusicManager.instance.gameObject.GetComponent<AudioSource>().pitch = Mathf.Lerp(MusicManager.instance.gameObject.GetComponent<AudioSource>().pitch, 0.5f, 0.1f);
		}
		else
		{
			if (MusicManager.instance != null)
				MusicManager.instance.gameObject.GetComponent<AudioSource>().pitch = Mathf.Lerp(MusicManager.instance.gameObject.GetComponent<AudioSource>().pitch, 1.5f, 0.025f);
		}

		//When all enemies are dead and all triggers are gone (Win)
		if (e == null && LevelManager.triggerCount <= 0)
		{
			winObject.SetActive(true);
			duelCamToSingleCam();
		}

		//When all players are dead (Lose)
		else if (cam1.gameObject.GetComponent<LerpToTransform>().plTr == null
		&& cam2.gameObject.GetComponent<LerpToTransform>().plTr == null)
		{
			loseObject.SetActive(true);
		}
		//ESC enables Pause state
		else if (Input.GetButtonDown(pauseInput))
		{
			if (!paused)
			{
				paused = true;
				duelCamToSingleCam();
				pauseObject.SetActive(true);
				Time.timeScale = 0f;
			}
			else
			{
				paused = false;
				pauseObject.GetComponent<PlaySound>().done = false;
				pauseObject.SetActive(false);
				Time.timeScale = 1f;
			}
		}
		//If game is paused
		else if (paused)
		{
			//Do nothing for now!
		}
		//If player 1 is gone
		else if (cam1.gameObject.GetComponent<LerpToTransform>().plTr == null)
		{
			duelCamToSingleCam(true);
			jointCam.GetComponent<LerpToTransform>().lerpPercent = 0.1f;

			//If the mode is VS. (Arena)
			if (!TogglesValues.coop)
			{
				winObject.SetActive(true);
				p2WinObject.SetActive(true);
			}
			//If the mode is Story
			else if (TogglesValues.story && !GameManager.singleGame)
			{
				loseObject.SetActive(true);
			}
		}
		//If player 2 is gone
		else if (cam2.gameObject.GetComponent<LerpToTransform>().plTr == null)
		{
			duelCamToSingleCam();
			jointCam.GetComponent<LerpToTransform>().lerpPercent = 0.1f;

			//If the mode is VS. (Arena)
			if (!TogglesValues.coop)
			{
				winObject.SetActive(true);
				p1WinObject.SetActive(true);
			}
			//If the mode is Story
			else if (TogglesValues.story && !GameManager.singleGame)
			{
				loseObject.SetActive(true);
			}
		}
		//If both player live and are in same trigger zone AND player preferred single window
		//OR if there is some dialogue to be shown on screen
		else if ((cam1.gameObject.GetComponent<LerpToTransform>().tr.position == cam2.gameObject.GetComponent<LerpToTransform>().tr.position
		&& TogglesValues.singleWindow)
		|| (dialogueBoxObject != null && dialogueBoxObject.GetComponent<SpriteRenderer>().enabled))
		{
			duelCamToSingleCam();
		}
		//If both player live and are in different trigger zones
		else
		{
			singleCamToDuelCam();
		}
	}
}
