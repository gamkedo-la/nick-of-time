using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    public GameObject Player;
    private WeaponPossession weaponPossession;

    public Image primaryEquipmentSlot;
    public Image secondaryEquipmentSlot;

    public TextMeshProUGUI primarySlotName;
    public TextMeshProUGUI secondarySlotName;

    Inventory inventory;

    private void Awake()
    {
        instance = this;
        weaponPossession = Player.GetComponentInChildren<WeaponPossession>();
        primaryEquipmentSlot.enabled = false;
        secondaryEquipmentSlot.enabled = false;
    }

    Equipment[] currentEquipment;

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        int numberOfSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        currentEquipment = new Equipment[numberOfSlots];
        inventory = Inventory.instance;
    }

    public void Equip(Equipment newItem) 
    {
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
            primaryEquipmentSlot.enabled = true;
            primaryEquipmentSlot.sprite = currentEquipment[slotIndex].icon;
            primarySlotName.text = currentEquipment[slotIndex].name;
        }

        if(slotIndex == 1)
        {
            secondaryEquipmentSlot.enabled = true;
            secondaryEquipmentSlot.sprite = currentEquipment[slotIndex].icon;
            secondarySlotName.text = currentEquipment[slotIndex].name;
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
        weaponPossession.weaponID = -1;
        primaryEquipmentSlot.sprite = null;
        primaryEquipmentSlot.enabled = false;
        primarySlotName.text = "Empty";
        secondaryEquipmentSlot.sprite = null;
        secondaryEquipmentSlot.enabled = false;
        secondarySlotName.text = "Empty";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }

}
