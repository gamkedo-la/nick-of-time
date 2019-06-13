using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;    

    public int damageModifier;

    public int weaponID;
    

    public override void Use()
    {
        base.Use();
        int index = equipmentManager.inventory.items.IndexOf(this);
        equipmentManager.Equip(this);
        equipmentManager.inventory.Remove(this);        
        equipmentManager.inventory.itemsInSlot.Remove(equipmentManager.inventory.itemsInSlot[index]);
        equipmentManager.inventory.onItemChangedCallback.Invoke();
    }
}

public enum EquipmentSlot { primaryWeapon, secondaryWeapon, primaryPotion, secondaryPotion}
