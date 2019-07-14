using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI itemName;

    [SerializeField]
    private Button thisItemButton;

    [SerializeField]
    private Button confirmTradeButton;
    
    [SerializeField]
    private TextMeshProUGUI numberOfItemsInSlotDisplay;

    public int numberOfItemsInStack = 0;

    public GameObject associatedPlayer;
    public GameObject otherPlayer;

    private Inventory inventory;

    private Inventory otherPlayerInventory;

    Item item;

    private int index;

    [SerializeField]
    private InventoryControlPanel inventoryControlPanel;

    [SerializeField]
    private TradePanel tradePanel;

    private void Awake()
    {
        itemName.text = "Empty";
        numberOfItemsInSlotDisplay.text = numberOfItemsInStack.ToString();
        inventory = associatedPlayer.GetComponent<Inventory>();
        otherPlayerInventory = otherPlayer.GetComponent<Inventory>();

        tradePanel = GetComponentInChildren<TradePanel>(true); //"true"overload Includes Inactive GameObjects

        inventoryControlPanel = GetComponentInParent<InventoryControlPanel>();

        //tradePanel.gameObject.SetActive(false);

        int index = inventory.items.IndexOf(item);
    }

    private void Update()
    {
        if (Input.GetButtonDown(TogglesValues.p1controller == "" ? "Inventory1" : "Inventory" + TogglesValues.p1controller) && inventoryControlPanel.IsTrading)
        {
            CancelTrade();
        }
        
        if(item == null)
        {
            numberOfItemsInSlotDisplay.enabled = false;
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        itemName.text = item.name;       
    }  

    public void AddToNumberInSlot(Item newItem)
    {
        item = newItem;
        int index = inventory.items.IndexOf(newItem);
        numberOfItemsInStack = inventory.itemsInSlot[index];
        numberOfItemsInSlotDisplay.enabled = true;

        if(numberOfItemsInStack <= 1)
        {
            numberOfItemsInSlotDisplay.text = "";
        }
        else
        {
            numberOfItemsInSlotDisplay.text = numberOfItemsInStack.ToString();
        }
    }

    public void ClearSlot() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        itemName.text = "Empty";
        numberOfItemsInStack = 0;
        numberOfItemsInSlotDisplay.text = numberOfItemsInStack.ToString();
        numberOfItemsInSlotDisplay.enabled = false;
    }

    public void OnRemoveButton()
    {
        inventory.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            if (item.name.Contains("Key"))
            {
                return;
            }

            if(inventoryControlPanel.IsTrading == false && inventoryControlPanel.IsDropping == false)
            {
                tradePanel.gameObject.SetActive(false);
                item.equipmentManager = inventory.equipmentManager;
                item.Use();
            }

            if (inventoryControlPanel.IsTrading)
            {
                tradePanel.gameObject.SetActive(true);
                inventory.onItemChangedCallback.Invoke();
                
               /* if(item.stackable == false)
                {
                    //tradePanel.amountToTradeDisplay.text = "";
                    tradePanel.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                }  */              

                confirmTradeButton.gameObject.SetActive(true);
                confirmTradeButton.Select();
                confirmTradeButton.OnSelect(null);
            }

			inventory.playInventoryUISound();
        }
    }

    public void ConfirmTrade()
    {
        if (item != null)
        {
            tradePanel.gameObject.SetActive(false);
            //tradePanel.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

            thisItemButton.Select();
            thisItemButton.OnSelect(null);
           
            inventory.Trade(item, inventory, otherPlayerInventory, 1);

            //tradePanel.amountToTrade = 0;
            //tradePanel.amountToTradeDisplay.text = tradePanel.amountToTrade.ToString();

            inventory.onItemChangedCallback.Invoke();
        }
        else
        {
            return;
        }
        
    }

    public void CancelTrade()
    {
        tradePanel.gameObject.SetActive(false);
        inventoryControlPanel.tradeButton.Select();
        inventoryControlPanel.tradeButton.OnSelect(null);
    }
}
