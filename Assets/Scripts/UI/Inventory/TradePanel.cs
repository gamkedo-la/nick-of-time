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
    private string VerticalAxisUpKey;
    [SerializeField]
    private string VerticalAxisDownKey;

    // Start is called before the first frame update
    void Start()
    {
       // this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(VerticalAxisUpKey))
        {
            amountToTrade += 1;
            amountToTradeDisplay.text = amountToTrade.ToString();
        }

        if (Input.GetKeyDown(VerticalAxisDownKey))
        {
            amountToTrade -= 1;
            amountToTradeDisplay.text = amountToTrade.ToString();
        }

        if(amountToTrade < 0)
        {
            amountToTrade = 0;
            amountToTradeDisplay.text = amountToTrade.ToString();
        }


    }

   

}
