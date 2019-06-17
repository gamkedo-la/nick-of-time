﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentManager : MonoBehaviour
{
    //public static EquipmentManager instance;

    public GameObject Player;
    public GameObject inventoryUI;
    private WeaponPossession weaponPossession;

    
    public Inventory inventory;
/*
    public Image primaryWeaponSlot;
    public Image secondaryWeaponSlot;
    public Image primaryPotionSlot;
    public Image secondaryPotionSlot;

    public TextMeshProUGUI primaryWeaponSlotName;
    public TextMeshProUGUI secondaryWeaponSlotName;
    public TextMeshProUGUI primaryPotionSlotName;
    public TextMeshProUGUI secondaryPotionSlotName;

    public TextMeshProUGUI primaryPotionSlotAmount;
    public TextMeshProUGUI secondaryPotionSlotAmount;*/

    [SerializeField]
    private string primaryPotion;
    [SerializeField]
    private string secondaryPotion;

    [SerializeField]
    public EquipmentUI equipmentUI;

    Equipment[] currentEquipment;

    private void Awake()
    {
		//instance = this;
		if (Player)
		{
			weaponPossession = Player.GetComponentInChildren<WeaponPossession>();
            
			//primaryWeaponSlot.enabled = false;
			//secondaryWeaponSlot.enabled = false;
		}
    }

    

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        int numberOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numberOfSlots];
        inventory = GetComponent<Inventory>();

        for (int i = 0; i < currentEquipment.Length; i++)
        {
            equipmentUI.equipmentSlotDisplays[i].equipmentIcon.enabled = false;
        }
    }

    public void Equip(Equipment newItem) 
    {
        newItem.equipmentManager = this;
        int slotIndex = (int)newItem.equipSlot;

        int index = inventory.items.IndexOf(newItem);

        Equipment oldItem = null;

        if(currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);
        }

        if(onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
        weaponPossession.weaponID = newItem.weaponID;
        currentEquipment[slotIndex] = newItem;

        equipmentUI.equipmentSlotDisplays[slotIndex].equipmentIcon.enabled = true;
        equipmentUI.equipmentSlotDisplays[slotIndex].equipmentIcon.sprite = currentEquipment[slotIndex].icon;
        equipmentUI.equipmentSlotDisplays[slotIndex].itemName.text = currentEquipment[slotIndex].name;
        if (newItem.stackable)
        {
            equipmentUI.equipmentSlotDisplays[slotIndex].numberOfItemsInStack.text = inventory.itemsInSlot[index].ToString();
        }
        else
        {
            equipmentUI.equipmentSlotDisplays[slotIndex].numberOfItemsInStack = null;
        }
        
        /*
        if (slotIndex == 0 )
        {
            primaryWeaponSlot.enabled = true;
            primaryWeaponSlot.sprite = currentEquipment[slotIndex].icon;
            primaryWeaponSlotName.text = currentEquipment[slotIndex].name;
        }

        if(slotIndex == 1)
        {
            secondaryWeaponSlot.enabled = true;
            secondaryWeaponSlot.sprite = currentEquipment[slotIndex].icon;
            secondaryWeaponSlotName.text = currentEquipment[slotIndex].name;
        }
        if (slotIndex == 2)
        {
            primaryPotionSlot.enabled = true;
            primaryPotionSlot.sprite = currentEquipment[slotIndex].icon;
            primaryPotionSlotName.text = currentEquipment[slotIndex].name;
            primaryPotionSlotAmount.text = inventory.itemsInSlot[index].ToString();
        }
        if (slotIndex == 3)
        {
            secondaryPotionSlot.enabled = true;
            secondaryPotionSlot.sprite = currentEquipment[slotIndex].icon;
            secondaryPotionSlotName.text = currentEquipment[slotIndex].name;
            secondaryPotionSlotAmount.text = inventory.itemsInSlot[index].ToString();
        }  */             
    }

    public void Unequip(int slotIndex)
    {
        if(currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];

            if(slotIndex == 0 || slotIndex == 1)
            {
                inventory.Add(oldItem);
            }

            currentEquipment[slotIndex] = null;


            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            equipmentUI.equipmentSlotDisplays[slotIndex].equipmentIcon.enabled = false;
            equipmentUI.equipmentSlotDisplays[slotIndex].equipmentIcon.sprite = null;
            equipmentUI.equipmentSlotDisplays[slotIndex].itemName.text = "Empty";
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
            weaponPossession.weaponID = -1;
            /*equipmentUI.equipmentSlotDisplays[i].equipmentIcon.enabled = false;
            equipmentUI.equipmentSlotDisplays[i].equipmentIcon.sprite = null;
            equipmentUI.equipmentSlotDisplays[i].itemName.text = "Empty";*/

            if(equipmentUI.equipmentSlotDisplays[i].numberOfItemsInStack != null)
            {
                equipmentUI.equipmentSlotDisplays[i].numberOfItemsInStack.text = "";
            }
        }

        //TODO: Clean up the following.  Can this be refactored to be based on the equipSlot index?  That way it would be more universal.
        
        /*primaryWeaponSlot.sprite = null;
        primaryWeaponSlot.enabled = false;
        primaryWeaponSlotName.text = "Empty";
        secondaryWeaponSlot.sprite = null;
        secondaryWeaponSlot.enabled = false;
        secondaryWeaponSlotName.text = "Empty";
        primaryPotionSlot.sprite = null;
        primaryPotionSlot.enabled = false;
        primaryPotionSlotName.text = "Empty";
        secondaryPotionSlot.sprite = null;
        secondaryPotionSlot.enabled = false;
        secondaryPotionSlotName.text = "Empty";

        primaryPotionSlotAmount.text = "";
        secondaryPotionSlotAmount.text = "";*/
    }

    public void UsePotion(string potionButton, Equipment potion)
    {
        int indexInEquipment = System.Array.IndexOf(currentEquipment, potion);      
        //TODO Refactor so this is more usable (Will have to copy this for Seconday Potion Use)
      
        int indexInInventory = inventory.items.IndexOf(potion);
        int amountOfPotions = inventory.itemsInSlot[indexInInventory];
        Debug.Log("Amount of potions " + amountOfPotions);
        
        if(amountOfPotions > 0)
        {
            inventory.itemsInSlot[indexInInventory] -= 1;
            equipmentUI.equipmentSlotDisplays[indexInEquipment].numberOfItemsInStack.text = inventory.itemsInSlot[indexInInventory].ToString();
            //primaryPotionSlotAmount.text = inventory.itemsInSlot[indexInInventory].ToString();            
        }
        if(amountOfPotions == 0)
        {
            inventory.itemsInSlot.RemoveAt(indexInInventory);
            inventory.items.Remove(potion); 
            Unequip(indexInEquipment);
                    
            
        }
        //inventory.onItemChangedCallback.Invoke();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }

        if (currentEquipment[2] != null && Input.GetButtonDown(primaryPotion))
        {
            UsePotion(primaryPotion, currentEquipment[2]);
            inventory.onItemChangedCallback.Invoke();
        }
        if (currentEquipment[3] != null && Input.GetButtonDown(secondaryPotion))
        {
            UsePotion(secondaryPotion, currentEquipment[3]);
            inventory.onItemChangedCallback.Invoke();
        }

    }

}
