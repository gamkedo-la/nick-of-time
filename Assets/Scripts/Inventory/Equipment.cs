using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;    

    public int damageModifier;

    public int weaponID = -1;
    

    public override void Use()
    {
        base.Use();
        int index = equipmentManager.inventory.items.IndexOf(this);
        equipmentManager.Equip(this);
        
        if(weaponID == 0 || weaponID == 1)
        {
            equipmentManager.inventory.itemsInSlot.RemoveAt(index);
            equipmentManager.inventory.Remove(this);            
        }
        equipmentManager.inventory.onItemChangedCallback.Invoke();
    }
}

public enum EquipmentSlot { primaryWeapon, secondaryWeapon, primaryPotion, secondaryPotion}
