using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool active = true;

	public Portal portalActivePair;

    public Portal[] exits;
    private List<int> activePortals;
    private int selectedIndex = 0;

    public GameObject player;

	public AudioClip portalSound;

    private string attackInput = "Fire";
    private string horizontalInput = "Horizontal";

    private LerpToTransform playerCamera;
    private Transform previousCameraTrack;
    private float previousPlayerFocusFactor;

	private AudioSource aud;

	private void Start()
	{
		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();
	}

	private void Update()
    {
		if (active && player)
		{
			// Confirm selection with attackInput
			if ((Input.GetButtonDown(attackInput + (player.name == "Player1" ? "1" : "2")))
			|| (GameManager.singleGame && Input.GetButtonDown(attackInput + "2")))
			{
				MinimapController.instances[0].focus = false;
				if (player.name == "Player1")
					MinimapController.instances[2].focus = false;
				else if (player.name == "Player2")
					MinimapController.instances[1].focus = false;
					
				if (aud != null && TogglesValues.sound)
					aud.PlayOneShot(portalSound);

				TransportPlayer(exits[activePortals[selectedIndex]]);
				playerCamera.tr = previousCameraTrack;
				playerCamera.playerFocusFactor = previousPlayerFocusFactor;
				playerCamera = null;
				StartCoroutine(ReactivatePlayer());
			}

			// Change selection with horizontalInput
			float selection = Input.GetAxisRaw(horizontalInput + (player.name == "Player1" ? "1" : "2"));
			if (selection == 0f && GameManager.singleGame)
				selection = Input.GetAxisRaw(horizontalInput + "2");

			if ((Input.GetButtonDown(horizontalInput + (player.name == "Player1" ? "1" : "2")))
			|| (GameManager.singleGame && Input.GetButtonDown(horizontalInput + "2")))
			{
				selectedIndex += (int)Mathf.Sign(selection);
				if (selectedIndex < 0)
				{
					selectedIndex = activePortals.Count - 1;
				}
				else if (selectedIndex >= activePortals.Count)
				{
					selectedIndex = 0;
				}
				PreviewSelection();
			}
		}
    }

    private void GetActivePortalIndices()
    {
        activePortals = new List<int>();
        for (var i = 0; i < exits.Length; i++)
        {
            if (exits[i].gameObject.activeInHierarchy)
            {
                activePortals.Add(i);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Player") && player == null)
        {
			// A Portal MUST lead to some other Portal
			//Make sure that portalActivePair is in the list of activePortals
			if(portalActivePair != null)
				portalActivePair.gameObject.SetActive(true);

			// update active portal index
			GetActivePortalIndices();

            // store reference to player
            player = collision.gameObject;
			
			// no exits, bail
			// @TODO: notify user somehow?
			if (activePortals.Count == 0)
            {
                player = null;
                return;
            }
            // if there's only one exit, just go there
            else if (activePortals.Count <= 2) //the second exit is the same portal itself
            {
                TransportPlayer(exits[activePortals[0]]);

				if (player.name == "Player1")
				{
					Subtitles.AddPlayer1Subtitle("Player Teleported!");
				}
				else if (player.name == "Player2")
				{
					Subtitles.AddPlayer2Subtitle("Player Teleported!");
				}
			}
            // otherwise start "selection mode"
            else
            {
                // Reset selectionIndex
                selectedIndex = 0;

                // Store inputs and disable player controller
                PlayerController playerController = player.GetComponent<PlayerController>();
                attackInput = playerController.attackInput;
                horizontalInput = playerController.walkHorizontalInput;
                playerController.enabled = false;

                // Move camera to show portal selection
                playerCamera = FindPlayerCamera();
                previousCameraTrack = playerCamera.tr;
                previousPlayerFocusFactor = playerCamera.playerFocusFactor;
                playerCamera.playerFocusFactor = 0f;
                PreviewSelection();

				//Minimap Focus and Subtitle
				MinimapController.instances[0].focus = true;
				if (player.name == "Player1")
				{
					MinimapController.instances[2].focus = true;
					Subtitles.AddPlayer1Subtitle("Select the Portal");
				}
				else if (player.name == "Player2")
				{
					MinimapController.instances[1].focus = true;
					Subtitles.AddPlayer2Subtitle("Select the Portal");
				}
			}
        }
    }

    // Reactivate on a delay to avoid PlayerController reading
    // input too early (don't punch out of the portal)
    private IEnumerator ReactivatePlayer()
    {
        yield return new WaitForEndOfFrame();
        if (player)
        {
            player.GetComponent<PlayerController>().enabled = true;
            player = null;
        }
    }

    // Re-enables the portal after the player has exited collision with it.
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            active = true;
			player = null;
        }
    }

    // Updates the player's camera to look at the selected portal.
    // Does *not* update the selectionIndex
    private void PreviewSelection()
    {
		if(exits != null && exits.Length > 0)
			playerCamera.tr = exits[activePortals[selectedIndex]].transform;
    }

    private void TransportPlayer(Portal exit)
    {
        exit.active = false;
        player.transform.position = exit.transform.position;
    }

    private LerpToTransform FindPlayerCamera()
    {
        foreach (LerpToTransform c in FindObjectsOfType<LerpToTransform>())
        {
            if (c.plTr && c.plTr.name == player.name)
            {
                return c;
            }
        }

        return null;
    }
}
