﻿using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;
    [SerializeField]
    private bool isLocked = false;
    [SerializeField]
    private GameObject childLock;
    [SerializeField]
    private GameObject childDoor;

    // Which key will unlock this door
    public Item key;

    private Animator lockAnimator;
    private Animator doorAnimator;

    private void Start()
    {
        lockAnimator = childLock.GetComponent<Animator>();
        doorAnimator = childDoor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        lockAnimator.SetBool("Locked", isLocked);
        doorAnimator.SetBool("Open", isOpen);
    }

    void OpenDoor()
    {
        isOpen = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player is near to the door
        if (collision.CompareTag("Player"))
        {
            // And has a key
            var inventory = collision.GetComponent<Inventory>();
            if (inventory.HasItem(key))
            {
                // Remove the key and unlock the door
                inventory.Remove(key);
                isLocked = false;
            }
        }
    }
}
