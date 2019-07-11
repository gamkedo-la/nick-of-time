using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public bool stackable = false; //Can you have more than one of this item in a slot? (Potions, bullets, coins etc.)    

    public EquipmentManager equipmentManager;

    public virtual void Use()
    {
        //Use the item
        Debug.Log("Using" + name);

		if (name == "Health Potion")
		{
			equipmentManager.Player.GetComponent<HitCheck>().hp += 0.25f;

			if (equipmentManager.Player.GetComponent<HitCheck>().hp > 1f)
				equipmentManager.Player.GetComponent<HitCheck>().hp = 1f;
		}
		else if (name == "Stamina Potion")
		{
			equipmentManager.Player.GetComponent<PlayerController>().actionPoints = 1f;
		}

        if (name.Contains("Key"))
        {
            return;
        }
    }

    /*public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }*/
}
