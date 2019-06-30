using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryControlPanel : MonoBehaviour
{

    public Button useButton;
    public Button tradeButton;
    public Button dropButton;

    private bool isTrading = false;

    private bool isDropping = false;

    public bool IsTrading { get => isTrading; set => isTrading = value; }

    public bool IsDropping { get => isDropping; set => isDropping = value; }

    public void Using()
    {
        isDropping = false;
        isTrading = false;
    }

    public void Trading()
    {
        isDropping = false;
        IsTrading = true;        
    }

    public void Dropping()
    {
        isDropping = true;
        IsTrading = false;
    }


}
