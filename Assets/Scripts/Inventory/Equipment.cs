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
        EquipmentManager.instance.Equip(this);
        RemoveFromInventory();
    }
}

public enum EquipmentSlot { primary, secondary}
