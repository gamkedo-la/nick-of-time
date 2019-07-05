using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;

    public GameObject inventoryUI;

    public GameObject associatedPlayer;

    public string inventoryOpen = "Inventory";

    Inventory inventory;

    InventorySlot[] slots;

    [SerializeField]
    private Button firstSelected;

    // Start is called before the first frame update
    void Start()
	{
		if (gameObject.name == "Player1")
		{
			if (TogglesValues.p1controller != "")
				inventoryOpen += TogglesValues.p1controller;
			else
				inventoryOpen += "1";
		}

		if (gameObject.name == "Player2")
		{
			if (TogglesValues.p2controller != "")
				inventoryOpen += TogglesValues.p2controller;
			else
				inventoryOpen += "2";
		}

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
        if (Input.GetButtonDown(inventoryOpen)
		|| (GameManager.singleGame && Input.GetButtonDown("Inventory2")))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);

            firstSelected?.Select();
            firstSelected?.OnSelect(null);

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
