using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;
    public TextMeshProUGUI textMeshPro;

    public GameObject associatedPlayer;

    private Inventory inventory;

    Item item;
    
    private void Awake()
    {
        textMeshPro.text = "Empty";
        inventory = associatedPlayer.GetComponent<Inventory>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        removeButton.interactable = true;
        textMeshPro.text = item.name;
    }

    public void ClearSlot() {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
        textMeshPro.text = "Empty";
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
