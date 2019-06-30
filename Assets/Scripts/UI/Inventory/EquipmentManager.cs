using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentManager : MonoBehaviour
{
    public GameObject Player;
    //public GameObject inventoryUI;
    //public GameObject equippedItems;
    public EquipmentUI equipmentUI;

    private WeaponPossession weaponPossession;
    
    public Inventory inventory;
    
    [SerializeField]
    private string primaryPotion;
    [SerializeField]
    private string secondaryPotion;

    Equipment[] currentEquipment;

    private void Awake()
    {
		//instance = this;
		if (Player)
		{
			weaponPossession = Player.GetComponentInChildren<WeaponPossession>();
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
        Player.GetComponent<Subtitles>().Caption("Equipped " + newItem.name);
        
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

            if(equipmentUI.equipmentSlotDisplays[i].numberOfItemsInStack != null)
            {
                equipmentUI.equipmentSlotDisplays[i].numberOfItemsInStack.text = "";
            }
        }
    }

    public void UsePotion(string potionButton, Equipment potion)
    {
        int indexInEquipment = System.Array.IndexOf(currentEquipment, potion);      
              
        int indexInInventory = inventory.items.IndexOf(potion);
        int amountOfPotions = inventory.itemsInSlot[indexInInventory];
        Debug.Log("Amount of potions " + amountOfPotions);
        
        if(amountOfPotions > 0)
        {
            inventory.itemsInSlot[indexInInventory] -= 1;
            equipmentUI.equipmentSlotDisplays[indexInEquipment].numberOfItemsInStack.text = inventory.itemsInSlot[indexInInventory].ToString();                       
        }
        if(amountOfPotions == 0)
        {
            inventory.itemsInSlot.RemoveAt(indexInInventory);
            inventory.items.Remove(potion); 
            Unequip(indexInEquipment);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }

        if (currentEquipment[1] != null && Input.GetButtonDown(primaryPotion))
        {
            UsePotion(primaryPotion, currentEquipment[1]);
            inventory.onItemChangedCallback.Invoke();
        }    

    }

}
