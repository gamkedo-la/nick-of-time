using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public Inventory inventory;

    public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp() {
        Debug.Log("Picking up " + item.name);
        //Add to inventory
        bool wasPickedUp = inventory.Add(item);

        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        inventory = collider.GetComponent<Inventory>();
        Debug.Log("collider belongs to " + collider.name);
        Debug.Log("Inventory belongs to " + inventory.gameObject.name);
        Interact();
    }

}
