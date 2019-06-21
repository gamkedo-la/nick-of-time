using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TradePanel : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI amountToTradeDisplay;

    [SerializeField]
    private Button increaseTradeAmountButton;
    [SerializeField]
    private Button decreaseTradeAmountButton;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            amountToTradeDisplay.text = "0";

            increaseTradeAmountButton.Select();
            increaseTradeAmountButton.OnSelect(null);

        }
    }

    public void ActivateTradePanel() {

    }

}
