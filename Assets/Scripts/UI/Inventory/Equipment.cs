using UnityEngine;

[CreateAssetMenu(fileName ="New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;    
	
    public int weaponID = -1;

    private Inventory inventory;

    public override void Use()
    {
        base.Use();
        
        inventory = equipmentManager.inventory;
        int index = inventory.items.IndexOf(this);
		
		if(!equipmentManager.IsItemEquipped(this))
			equipmentManager.Equip(this);
        
        if(weaponID >= 0)
        {
            equipmentManager.inventory.itemsInSlot.RemoveAt(index);
            equipmentManager.inventory.Remove(this);            
        }
        equipmentManager.inventory.onItemChangedCallback.Invoke();
    }
}

public enum EquipmentSlot { primaryWeapon, primaryPotion }
