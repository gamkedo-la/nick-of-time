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

            items.Add(item);
            item.equipmentManager = GetComponent<EquipmentManager>();
            if(onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();

            }
        }
        return true;
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
