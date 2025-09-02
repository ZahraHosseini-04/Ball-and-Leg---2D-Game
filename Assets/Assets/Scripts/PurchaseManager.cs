using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bazaar.Data;
using Bazaar.Poolakey;
using Bazaar.Poolakey.Data;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static PurchaseManager Instance { get; private set; }
    public string appkey = "";
    private Payment _payment;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    async void Start()
    {
        var isSuccess = await Init();
         
    }

    public async Task<bool> Init()
    {
        var securityCheck = SecurityCheck.Enable(appkey);
        var PaymentConfig = new PaymentConfiguration(securityCheck);
        _payment = new Payment(PaymentConfig);
        var result = await _payment.Connect();

        return result.status == Status.Success;

    }
    public async Task<bool> OnPurchase(string productID)
    {
        var result = await _payment.Purchase(productID);
        if (result.status == Status.Success)
        {
            var resultConsume = await _payment.Consume(result.data.purchaseToken);
            if (resultConsume.status == Status.Success)
            {
                return true;
            }

        }
        return false;

    }

    // public async Task<Result<bool>> Consume(string purchaseId)
    // {
    //     var result = await _payment.Consume(purchaseId);
    //     return result;

    // }
    void OnPaymentConnect(Result<bool> result)
    { 
        
     }

    // void OnReceivePurchases(Result<List<PurchaseInfo>> result)
    // { 
    //     if (result.status == Status.Success)
    //     {
    //         foreach (var purchase in result.data)
    //         {
    //             Debug.Log($"Purchase ID: {purchase.purchaseId}, Product ID: {purchase.productId}, Status: {purchase.status}");
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogError("Failed to retrieve purchases: " + result.error);
    //     }
        
    //   }

    void OnApplicationQuit()
{
    _payment.Disconnect();
}

}
