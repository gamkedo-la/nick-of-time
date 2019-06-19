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

    [SerializeField]
    private Button firstSelected;

    // Start is called before the first frame update
    void Start()
    {
		if (associatedPlayer != null)
		{
			inventory = associatedPlayer.GetComponent<Inventory>();
			inventory.onItemChangedCallback += UpdateUI;

			slots = itemsParent.GetComponentsInChildren<InventorySlot>();
		}

        inventoryUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(inventoryOpen))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            firstSelected.Select();
            firstSelected.OnSelect(null);

            //slots[0].GetComponentInChildren<Button>().Select();
        }
		if (inventoryUI != null)
		{
			if (inventoryUI.activeSelf)
			{
				GetComponent<PlayerController>().enabled = false;
			}
			else
			{
				GetComponent<PlayerController>().enabled = true;
			}
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
                slots[i].AddToNumberInSlot(inventory.items[i]);               
            }
            else
            {
                slots[i].ClearSlot();
            }            
        }
    }

}
