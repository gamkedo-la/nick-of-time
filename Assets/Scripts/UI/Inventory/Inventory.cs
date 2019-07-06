using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public EquipmentManager equipmentManager;

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 8;

    public List<Item> items = new List<Item>();
    public List<int> itemsInSlot = new List<int>();

    public void Awake()
    {
        equipmentManager = GetComponent<EquipmentManager>();
    }


    
    public bool Add(Item item)
    {
        if(item != item.isDefaultItem)
        {
            if(items.Count >= space)
            {
                Debug.Log("Inventory full");
                return false;
            }

            if(item.stackable == false)
            {
                items.Add(item);
                itemsInSlot.Add(1);
            }

            if (HasItem(item) == false && item.stackable == true)
            {
                items.Add(item);
                itemsInSlot.Add(0);
            }                  

            item.equipmentManager = GetComponent<EquipmentManager>();
            if(onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
        return true;
    }
    
    public void AddToStack(Item item, int amount)
    {
        int index = items.IndexOf(item);
        Debug.Log(item.name + "is in slot " + index);
        
        if((itemsInSlot[index] + amount) >= 32)
        {
            itemsInSlot[index] = 32;
        }
        else
        {
            itemsInSlot[index] += amount;
        }
        Debug.Log("There are " + itemsInSlot[index].ToString() + " in slot " + index);
        onItemChangedCallback.Invoke();
    }

    public bool HasItem(Item item)
    {
        return items.Exists(i => i == item);  
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();

        }
    }

    public void Trade(Item itemToTrade, Inventory givingInventory, Inventory recievingInventory, int amountToTrade)
    {
        int index = items.IndexOf(itemToTrade);

        if (itemToTrade.stackable == false)
        {
            givingInventory.Remove(itemToTrade);
            recievingInventory.Add(itemToTrade);

            givingInventory.itemsInSlot.RemoveAt(index);
            recievingInventory.itemsInSlot.Add(recievingInventory.itemsInSlot[index]);
        }

        if(itemToTrade.stackable == true)
        {
            if(amountToTrade >= itemsInSlot[index])
            {
                amountToTrade = itemsInSlot[index];
            }
            givingInventory.AddToStack(itemToTrade, -amountToTrade);
            if(recievingInventory.HasItem(itemToTrade) == true)
            {
                recievingInventory.AddToStack(itemToTrade, amountToTrade);
            }
            if(recievingInventory.HasItem(itemToTrade) == false)
            {
                recievingInventory.Add(itemToTrade);
                recievingInventory.AddToStack(itemToTrade, amountToTrade);
            }

            if (itemsInSlot[index] <= 0)
            {
                givingInventory.Remove(itemToTrade);
                givingInventory.itemsInSlot.RemoveAt(index);
            }
        }
        onItemChangedCallback.Invoke();
    }
}
