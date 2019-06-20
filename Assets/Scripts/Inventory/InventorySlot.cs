using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI itemName;
    
    [SerializeField]
    private TextMeshProUGUI numberOfItemsInSlotDisplay;

    public int numberOfItemsInStack = 0;

    public GameObject associatedPlayer;

    private Inventory inventory;

    Item item;

    [SerializeField]
    private InventoryControlPanel inventoryControlPanel;

    [SerializeField]
    private TradePanel tradePanel;

    private void Awake()
    {
        itemName.text = "Empty";
        numberOfItemsInSlotDisplay.text = numberOfItemsInStack.ToString();
        inventory = associatedPlayer.GetComponent<Inventory>();
        inventoryControlPanel = GetComponentInParent<InventoryControlPanel>();
       

        tradePanel.gameObject.SetActive(false);
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
        numberOfItemsInSlotDisplay.text = numberOfItemsInStack.ToString();
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
        if(item != null)
        {
            if(inventoryControlPanel.IsTrading == false && inventoryControlPanel.IsDropping == false)
            {
                item.equipmentManager = inventory.equipmentManager;
                item.Use();
            }

            if (inventoryControlPanel.IsTrading)
            {
                tradePanel.gameObject.SetActive(true);
            }
            
          /*  if (item.stackable)   
            {
                numberOfItemsInStack -= 1;
            }*/
        }
        
    }
}
