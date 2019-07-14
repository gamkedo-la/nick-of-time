using System;
using UnityEngine;

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

    Equipment[] currentEquipment;

    private void Awake()
    {
		//instance = this;
		if (Player)
		{
			weaponPossession = Player.GetComponentInChildren<WeaponPossession>();
		}
    }

	public Equipment[] GetCurrentEquipment()
	{
		return currentEquipment;
	}

    internal void IfPotionIsEquippedUpdateItemCount(Item item)
    {
        int indexInInventory = inventory.items.IndexOf(item);

        if (currentEquipment[1] == null || currentEquipment[1].name != item.name)
        {
            return;
        }

        equipmentUI.equipmentSlotDisplays[1].numberOfItemsInStack.text = 
            inventory.itemsInSlot[indexInInventory].ToString();
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

	public bool IfNoWeaponEquip(Equipment item)
	{
		if (weaponPossession.weaponID <= -1 && item.weaponID > -1)
		{
			Equip(item);
			return true;
		}

		return false;
	}

    public void Equip(Equipment newItem) 
    {
		if(Player.name == "Player1")
			Subtitles.AddPlayer1Subtitle("Equipped " + newItem.name);
		else
			Subtitles.AddPlayer2Subtitle("Equipped " + newItem.name);

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

    public void UsePotion(Equipment potion)
    {
        int indexInEquipment = System.Array.IndexOf(currentEquipment, potion);      
              
        int indexInInventory = inventory.items.IndexOf(potion);
        int amountOfPotions = inventory.itemsInSlot[indexInInventory];
        Debug.Log("Amount of potions " + amountOfPotions);
        
        if(amountOfPotions > 0)
        {
			inventory.items[indexInInventory].Use();
            inventory.itemsInSlot[indexInInventory] -= 1;
            amountOfPotions = inventory.itemsInSlot[indexInInventory];

            equipmentUI.equipmentSlotDisplays[indexInEquipment].numberOfItemsInStack.text = inventory.itemsInSlot[indexInInventory].ToString();

            if (amountOfPotions == 0)
            {
                Debug.Log("Index of Potion is " + indexInInventory);
                Debug.Log("There are zero potions and this is removed");
                Unequip(indexInEquipment);
                inventory.Remove(potion);
                inventory.itemsInSlot.RemoveAt(indexInInventory);
				equipmentUI.equipmentSlotDisplays[indexInEquipment].numberOfItemsInStack.text = "";
				
				inventory.onItemChangedCallback.Invoke();
            }
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }

		/*
        if (currentEquipment[1] != null && Input.GetButtonDown(primaryPotion))
        {
            UsePotion(currentEquipment[1]);
            inventory.onItemChangedCallback.Invoke();
        }    
		*/
    }

}
