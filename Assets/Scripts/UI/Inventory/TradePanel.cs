using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TradePanel : MonoBehaviour
{

    public int amountToTrade;

    [SerializeField]
    private TextMeshProUGUI amountToTradeDisplay;

    [SerializeField]
    private Button increaseTradeAmountButton;
    [SerializeField]
    private Button decreaseTradeAmountButton;
    [SerializeField]
    private Button tradeButton;
    [SerializeField]
    private Button cancelButton;

    [SerializeField]
    private string VerticalAxis;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis(VerticalAxis) > 0.5f)
        {
            amountToTrade += 1;
            amountToTradeDisplay.text = amountToTrade.ToString();
        }

        if (Input.GetAxis(VerticalAxis) < -0.5f)
        {
            amountToTrade -= 1;
            amountToTradeDisplay.text = amountToTrade.ToString();
        }

    }

    public void ActivateTradePanel() {
        Debug.Log("Trade panel opened");
        amountToTradeDisplay.text = "0";

        tradeButton.Select();
        tradeButton.OnSelect(null);
    }

}
