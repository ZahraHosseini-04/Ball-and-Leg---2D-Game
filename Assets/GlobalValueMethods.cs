using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static partial class GlobalValue
{
    public static bool GetItems(string key)
    {
        if (PlayerPrefs.GetString(key, "Locked")=="Locked")
        {
            return false;
        }
        else
        {
            return true;
        }
 
    }

    public static void WhichBallChoose(string key)
    {
        PlayerPrefs.SetString(ChoosenBall, key);
    }

    public static bool[] GetAllItems()
    {
        bool[] items = new bool[ItemBallKeys.Length];
        for (int i = 0; i < ItemBallKeys.Length; i++)
        {
            items[i] = GetItems(ItemBallKeys[i]);
        }
        return items;
    }
    public static void AddItems(string key, int value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int currentValue = PlayerPrefs.GetInt(key);
            PlayerPrefs.SetInt(key, currentValue + value);
        }
        else
        {
            PlayerPrefs.SetInt(key, value);
        }
    }
    public static bool ConsumeItems(string key, int value)
    {
        if (PlayerPrefs.HasKey(key))
        {
            int currentValue = PlayerPrefs.GetInt(key);
            if (currentValue >= value)
            {
                PlayerPrefs.SetInt(key, currentValue - value);
                return true;
            }
            else
            {
                Debug.LogWarning($"Not enough {key} to consume.");
                return false;
            }
        }
        else
        {
            Debug.LogWarning($"{key} does not exist.");
            return false;
        }
    }
    public static void UnlockItem(string Item)
    {
        PlayerPrefs.SetString(Item,"Unlocked");
    }
}
