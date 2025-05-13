using System;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataCreatePlayer
{
    public long createdTimestamp = 0;
    public int interstitialAds = 0;
    public int numberAds = 0;

    public bool isRemoveAds = false;
    public Inventory inventoryNew = new Inventory();


    public Inventory Inventory
    {
        get { return inventoryNew; }
        set { inventoryNew = value; }
    }

    public long CreatedTimestamp
    {
        get { return createdTimestamp; }
        set
        {
            createdTimestamp = value;
        }
    }

    public int NumberAds
    {
        get { return numberAds; }
        set
        {
            numberAds = value;
        }
    }

    public int InterstititialAdsCount
    {
        get { return interstitialAds; }
        set
        {
            interstitialAds = value;
        }
    }
}
