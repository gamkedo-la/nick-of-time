﻿using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    public GameObject associatedPlayer;

    Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = associatedPlayer.GetComponent<Inventory>();
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Inventory1"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
            
                slots[0].GetComponentInChildren<Button>().Select();            
        }
    }

    void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        for (int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

}
