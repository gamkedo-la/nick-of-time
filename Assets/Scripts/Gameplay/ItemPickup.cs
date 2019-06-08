﻿using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        inventory = collider.GetComponent<Inventory>();
        Debug.Log("collider belongs to " + collider.name);
        Debug.Log("Inventory belongs to " + inventory.gameObject.name);
        Interact();
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		inventory = collision.gameObject.GetComponent<Inventory>();
		Debug.Log("collision belongs to " + collision.gameObject.name);
		Debug.Log("Inventory belongs to " + inventory.gameObject.name);
		Interact();
	}

}
