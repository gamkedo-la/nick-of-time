using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryControlPanel : MonoBehaviour
{

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
