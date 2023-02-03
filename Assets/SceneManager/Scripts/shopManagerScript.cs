using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class shopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[5, 5];
    public Text coinsTxt;


    void Start()
    {
        coinsTxt.text = "Coins: " + MoneyManager.Instance.GetMoney().ToString();

        ///Item ID's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;

        ///Price of item
        shopItems[2, 1] = 10;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 30;
        shopItems[2, 4] = 40;

        ///Quantity of item
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
    }


    public void Buy()
    {
        int coins = MoneyManager.Instance.GetMoney();
        ButtonInfo ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject.GetComponent<ButtonInfo>();
        
        // less then item shop price
        if (coins < shopItems[2, ButtonRef.ItemID])
        {
            return;
        }

        // coins greater or equal to price of item
        if (coins >= shopItems[2, ButtonRef.ItemID])
        {
            // prevent axe from being bought twice
            if (shopItems[3, 1] == ButtonRef.ItemID) return;

            // reduce money from item price
            coins -= shopItems[2, ButtonRef.ItemID];
            // increase quanity amount
            shopItems[3, ButtonRef.ItemID]++;

            // update money ui
            coinsTxt.text = "Coins: " + coins.ToString();
            // update quanity ui
            ButtonRef.quantityTxt.text = shopItems[3, ButtonRef.ItemID].ToString();
        }
    }
}
