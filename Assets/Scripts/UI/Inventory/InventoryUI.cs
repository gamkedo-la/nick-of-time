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

	private float prevTimeScale = 1f;

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
			
			if (GameManager.singleGame)
				MinimapController.instances[0].focus = inventoryUI.activeSelf;

			if (associatedPlayer.name == "Player1")
			{
				MinimapController.instances[2].focus = inventoryUI.activeSelf;
				
				if (!GameManager.singleGame)
				{
					MinimapController.instances[0].noLerp = inventoryUI.activeSelf;
					
					Vector3 pos = MinimapController.instances[0].transform.localPosition;

					if (MinimapController.instances[0].noLerp)
						pos.x = Mathf.Abs(pos.x);
					else
						pos.x = -Mathf.Abs(pos.x);

					MinimapController.instances[0].transform.localPosition = pos;
				}
			}
			else if (associatedPlayer.name == "Player2")
			{
				MinimapController.instances[1].focus = inventoryUI.activeSelf;
			}

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
