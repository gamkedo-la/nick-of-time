using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    public GameObject associatedPlayer;

    public string inventoryOpen = "Inventory1";

    Inventory inventory;

    InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = associatedPlayer.GetComponent<Inventory>();
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(inventoryOpen))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            slots[0].GetComponentInChildren<Button>().Select();
        }
        if (inventoryUI.activeSelf)
        {
            GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            GetComponent<PlayerController>().enabled = true;
        }

    }

    void UpdateUI()
    {
        Debug.Log("UPDATING UI");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

}
