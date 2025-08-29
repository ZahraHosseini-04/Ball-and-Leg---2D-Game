using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    public Button Init;
    public Button Consume;
    public Button purchase;

    public PurchaseManager purchaseManager;
    private bool isinit = false;
     
    private const string PURCHASEID = "SEKEH10";
    // Start is called before the first frame update
    async void Start()
    {
        var isSuccess = await purchaseManager.Init();
        purchase.onClick.AddListener(OnPurchaseClick);
        Consume.interactable = false;
        purchase.interactable = false;
        if (isSuccess)
        {
            isinit = true;
            Consume.interactable = true;
            purchase.interactable = true;
        }
    }

    private async void OnPurchaseClick()
    {
        var result = await purchaseManager.OnPurchase(PURCHASEID);
        //Debug.Log("amir" + result.data.purchaseToken + "" + result.data.productId);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
