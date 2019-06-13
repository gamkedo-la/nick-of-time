using System.Collections;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool active = true;

    public Portal[] exits;
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
            // Confirm selection with attackInput
            if (Input.GetButtonDown(attackInput))
            {
                TransportPlayer(exits[selectedIndex]);
                playerCamera.tr = previousCameraTrack;
                playerCamera.playerFocusFactor = previousPlayerFocusFactor;
                playerCamera = null;
                StartCoroutine(ReactivatePlayer());
            }

            // Change selection with horizontalInput
            float selection = Input.GetAxisRaw(horizontalInput);
            if (Input.GetButtonDown(horizontalInput))
            {
                selectedIndex += (int) Mathf.Sign(selection);
                if (selectedIndex < 0)
                {
                    selectedIndex = exits.Length - 1;
                }
                else if (selectedIndex >= exits.Length)
                {
                    selectedIndex = 0;
                }
                PreviewSelection();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Player"))
        {
            // store reference to player
            player = collision.gameObject;

            // if there's only one exit, just go there
            if (exits.Length == 1)
            {
                TransportPlayer(exits[0]);
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
			playerCamera.tr = exits[selectedIndex].transform;
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
