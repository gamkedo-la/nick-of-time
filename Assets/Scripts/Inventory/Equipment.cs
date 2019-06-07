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
        equipmentManager.Equip(this);
        equipmentManager.inventory.Remove(this);
    }
}

public enum EquipmentSlot { primaryWeapon, secondaryWeapon, primaryPotion, secondaryPotion}
