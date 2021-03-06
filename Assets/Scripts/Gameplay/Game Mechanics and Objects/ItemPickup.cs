﻿using UnityEngine;

public class ItemPickup : Interactable
{
    public Item item;

    public Inventory inventory;

    public int amountInStack = 0;

	public bool destroy = true;

	public AudioClip pickupSound;

	private AudioSource aud;

	private void Start()
	{
		aud = GetComponent<AudioSource>();
		if (aud == null)
			aud = FindObjectOfType<AudioSource>();
	}

	public override void Interact()
    {
        base.Interact();

        PickUp();
    }

    void PickUp() {
		Debug.Log("Picking up " + item.name);

		//Add to inventory
		bool wasPickedUp = false;

		if (inventory)
		{
			wasPickedUp = inventory.Add(item);

			if (item.name.Contains("Dagger") || item.name.Contains("Katana")
			|| item.name.Contains("Spear") || item.name.Contains("Whip")) //basically any weapon
			{
				item.equipmentManager.IfNoWeaponEquip((Equipment)item);
			}
			else if (item.stackable)
			{
				inventory.AddToStack(item, amountInStack);
				item.equipmentManager.IfPotionIsEquippedUpdateItemCount(item);
			}
		}

        if (wasPickedUp)
        {
			if (pickupSound != null && aud != null && TogglesValues.sound)
				aud.PlayOneShot(pickupSound);

			if (destroy)
				Destroy(gameObject);
			else
			{
				SpriteRenderer sprRend = GetComponent<SpriteRenderer>();
				if (sprRend != null) Destroy(sprRend);

				Destroy(this);
			}
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        inventory = collider.GetComponentInParent<Inventory>();
        Debug.Log("collider belongs to " + collider.name);

		if (inventory)
			Debug.Log("Inventory belongs to " + inventory.gameObject.name);

		if (collider.gameObject.name == "Player1")
			Subtitles.AddPlayer1Subtitle("Picked up " + item.name);
		else if (collider.gameObject.name == "Player2")
			Subtitles.AddPlayer2Subtitle("Picked up " + item.name);

		Interact();
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		inventory = collision.gameObject.GetComponent<Inventory>();
		Debug.Log("collision belongs to " + collision.gameObject.name);

		if(inventory)
			Debug.Log("Inventory belongs to " + inventory.gameObject.name);

		if (collision.gameObject.name == "Player1")
			Subtitles.AddPlayer1Subtitle("Picked up " + item.name);
		else if (collision.gameObject.name == "Player2")
			Subtitles.AddPlayer2Subtitle("Picked up " + item.name);

		Interact();
	}

}
