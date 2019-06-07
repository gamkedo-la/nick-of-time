using System.Collections;
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

    public Image primaryWeaponSlot;
    public Image secondaryWeaponSlot;
    public Image primaryPotionSlot;
    public Image secondaryPotionSlot;

    public TextMeshProUGUI primaryWeaponSlotName;
    public TextMeshProUGUI secondaryWeaponSlotName;
    public TextMeshProUGUI primaryPotionSlotName;
    public TextMeshProUGUI secondaryPotionSlotName;


   

    private void Awake()
    {
        //instance = this;
        weaponPossession = Player.GetComponentInChildren<WeaponPossession>();
        primaryWeaponSlot.enabled = false;
        secondaryWeaponSlot.enabled = false;
    }

    Equipment[] currentEquipment;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        int numberOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numberOfSlots];
        inventory = GetComponent<Inventory>();
    }

    public void Equip(Equipment newItem) 
    {
        newItem.equipmentManager = this;
        int slotIndex = (int)newItem.equipSlot;

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

        if(slotIndex == 0 )
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
        }
        if (slotIndex == 3)
        {
            secondaryPotionSlot.enabled = true;
            secondaryPotionSlot.sprite = currentEquipment[slotIndex].icon;
            secondaryPotionSlotName.text = currentEquipment[slotIndex].name;
        }
    }

    public void Unequip(int slotIndex)
    {
        if(currentEquipment[slotIndex] != null)
        {
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
        }
    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }

        //TODO: Clean up the following
        weaponPossession.weaponID = -1;
        primaryWeaponSlot.sprite = null;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

}
