using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI itemName;

    private int numberOfItemsInSlot = 0;

    [SerializeField]
    private TextMeshProUGUI numberOfItemsInSlotDisplay;

    public GameObject associatedPlayer;

    private Inventory inventory;

    Item item;
    
    private void Awake()
    {
        itemName.text = "Empty";
        numberOfItemsInSlotDisplay.text = numberOfItemsInSlot.ToString();
        inventory = associatedPlayer.GetComponent<Inventory>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        itemName.text = item.name;
        if(newItem.stackable == true)
        {
            numberOfItemsInSlot += 1;
            numberOfItemsInSlotDisplay.enabled = true;
            numberOfItemsInSlotDisplay.text = numberOfItemsInSlot.ToString();
        }
    }

    public void ClearSlot() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        itemName.text = "Empty";
    }

    public void OnRemoveButton()
    {
        inventory.Remove(item);
    }

    public void UseItem()
    {
        if(item != null)
        {
            item.Use();            
        }
    }
}
