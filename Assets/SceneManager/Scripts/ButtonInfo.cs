using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{

    public int ItemID;
    public Text priceTxt;
    public Text quantityTxt;
    public GameObject shopManager;

    



    // Update is called once per frame
    void Update()
    {
        priceTxt.text = "Price: $" + shopManager.GetComponent<shopManagerScript>().shopItems[2,ItemID].ToString();
        quantityTxt.text =  shopManager.GetComponent<shopManagerScript>().shopItems[3,ItemID].ToString();
    }
}
