
using UnityEngine;
[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Potion")]
public class Potion : Equipment
{
    
    public string potionButton;
       
     void Update()
    {
        if (Input.GetButtonDown(potionButton))
        {
            Debug.Log("Potion Called");
           // UsePotion();
            
        }
    }

   /* public void UsePotion()
    {
        inventory = equipmentManager.inventory;        
        int index = equipmentManager.inventory.items.IndexOf(this);
        int itemsInStack =  equipmentManager.inventory.itemsInSlot[index];

        inventory.AddToStack(this, -1);
    }*/

}
