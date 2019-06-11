using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public bool isDefaultItem = false;

    public bool stackable = false; //Can you have more than one of this item in a slot? (Potions, bullets, coins etc.)

    public int numberOfItemsInStack = 1;

    public EquipmentManager equipmentManager;

    public virtual void Use()
    {
        //Use the item
        Debug.Log("Using" + name);
    }

    /*public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }*/
}
