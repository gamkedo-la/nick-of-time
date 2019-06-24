
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public virtual void Interact()
    {
        //This method is meant to be overwritten
        //Figure out how you want to call this function (trigger, click, select button etc.)
        Debug.Log("Interacting with " + transform.name);
    }

}
