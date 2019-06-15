using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool active = true;

    public Portal[] exits;
    private List<int> activePortals;
    private int selectedIndex = 0;

    public GameObject player;
    private string attackInput = "Fire1";
    private string horizontalInput = "Horizontal";

    private LerpToTransform playerCamera;
    private Transform previousCameraTrack;
    private float previousPlayerFocusFactor;

    private void Update()
    {
		if (player)
		{
			MinimapController.instances[0].focus = true;
			if (player.name == "Player1")
				MinimapController.instances[1].focus = true;
			else if (player.name == "Player2")
				MinimapController.instances[2].focus = true;

			// Confirm selection with attackInput
			if (Input.GetButtonDown(attackInput))
			{
				MinimapController.instances[0].focus = false;
				if (player.name == "Player1")
					MinimapController.instances[1].focus = false;
				else if (player.name == "Player2")
					MinimapController.instances[2].focus = false;

				TransportPlayer(exits[activePortals[selectedIndex]]);
				playerCamera.tr = previousCameraTrack;
				playerCamera.playerFocusFactor = previousPlayerFocusFactor;
				playerCamera = null;
				StartCoroutine(ReactivatePlayer());
			}

			// Change selection with horizontalInput
			float selection = Input.GetAxisRaw(horizontalInput);
			if (Input.GetButtonDown(horizontalInput))
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
            else if (activePortals.Count == 1)
            {
                TransportPlayer(exits[activePortals[0]]);
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
