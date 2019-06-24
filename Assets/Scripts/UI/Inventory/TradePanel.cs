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
    [SerializeField]
    private Button tradeButton;
    [SerializeField]
    private Button cancelButton;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void ActivateTradePanel() {
        Debug.Log("Trade panel opened");
        amountToTradeDisplay.text = "0";

        tradeButton.Select();
        tradeButton.OnSelect(null);
    }

}
