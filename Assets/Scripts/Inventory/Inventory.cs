using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /*
# region Singleton

    public static Inventory instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion*/

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 8;

    public List<Item> items = new List<Item>();

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
            }

            if (HasItem(item) == false && item.stackable == true)
            {
                items.Add(item);
            }


            
            if(HasItem(item) && item.stackable)
            {
                item.numberOfItemsInStack += item.numberOfItemsInStack;
            }

            item.equipmentManager = GetComponent<EquipmentManager>();
            if(onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();

            }
        }
        return true;
    }

    public bool AddToItemAmount(Item item, int amountToAdd)
    {
        if (HasItem(item))
        {
            //Add to the amount of items in slot instead of adding another item to list.             
        }
        return true;
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
}
