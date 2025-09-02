using System;
using System.Collections;
using System.Collections.Generic;

// using TapsellPlusSDK;
using AdiveryUnity;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }
    // Start is called before the first frame update
    public string APP_ID = "";
    public string ZONE_ID_REWARDEDAD = "";
    AdiveryListener listener;
    public static event Action RewardGranted;


    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Adivery.Configure(APP_ID);
        Adivery.PrepareRewardedAd(ZONE_ID_REWARDEDAD);

    listener = new AdiveryListener();

    listener.OnError += OnError;
    listener.OnRewardedAdLoaded += OnRewardedLoaded;
    listener.OnRewardedAdClosed += OnRewardedClosed;

    Adivery.AddListener(listener);

    }

    // Update is called once per frame
public void ShowRewardedAd()
    {
  if (Adivery.IsLoaded(ZONE_ID_REWARDEDAD)){
    Adivery.Show(ZONE_ID_REWARDEDAD);
}
    }
public void OnRewardedLoaded(object caller, string placementId)
{
    // Rewarded ad loaded
}

    public void OnRewardedClosed(object caller, AdiveryReward reward)
    {
        // Check if User should receive the reward
        if (reward.IsRewarded)
        {
            getRewardAmount(reward.PlacementId); // Implrement getRewardAmount yourself
        }
        else
        {
        getRewardAmount(reward.PlacementId); 
    }
}

    private void getRewardAmount(string placementId)
    {
        // throw new NotImplementedException();
        GiveReward();
    }

    public void OnError(object caller, AdiveryUnity.AdiveryError args)
    {
        // log optional
    }

    private void GiveReward()
    {
     
        RewardGranted?.Invoke();
    }

    
}
