using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal[] exits;
    bool active = true;

    private void TransportPlayer(GameObject player, Portal exit)
    {
        exit.active = false;
        player.transform.position = exit.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && collision.CompareTag("Player"))
        {
            if (exits.Length == 1)
            {
                TransportPlayer(collision.gameObject, exits[0]);
            }
            else
            {
                // @TODO: Display portal options
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        active = true;
    }
}
