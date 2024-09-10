using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalAndCloudDataService
{
    // ChangedData
    private int moneyCount = 0;
    private List<Weapon> purchasedWeapons = new List<Weapon>();
    private bool showAd = false;
    private int currentLevel = 0;
    private Weapon currentWeapon;

    // PermanentData
    private Dictionary<Weapon, WeaponInfo> weaponAndWeaponsInfo = new Dictionary<Weapon, WeaponInfo>();

    private Dictionary<int, LevelInfo> numberAndLevelInfo = new Dictionary<int, LevelInfo>();






    public LocalAndCloudDataService()
    {

    }

    public void SaveOnCloudData()
    {
        //
    }
    public void LoadFromCloudData()
    {
        //
    }

    // Money
    public int GetCurrentMoney()
    {
        return moneyCount;
    }

    public void ChangeCurrentMoney(int value)
    {
        moneyCount = Mathf.Clamp(moneyCount + value, 0, 9999);
        SaveOnCloudData();
    }

    // Weapon
    public Dictionary<Weapon, WeaponInfo> GetWeaponDictionary()
    {
        return weaponAndWeaponsInfo;
    }

    public void SetWeaponPurchasedStatus(Weapon weapon)
    {
        if (!purchasedWeapons.Contains(weapon))
        {
            purchasedWeapons.Add(weapon);
        }
        SaveOnCloudData();
    }
    // Ad
    public void SetAdActivity(bool value)
    {
        showAd = value;
    }

    public bool GetAdActivity()
    {
        return showAd;
    }
    // Change Current Level
    public void ChangeCurrentLevel(int value)
    {
        currentLevel = Mathf.Clamp(value, 0, 50);
    }



}

[Serializable]
public class WeaponInfo
{
    public string weaponCost;
    public Sprite weaponIcon;
    public Sprite weaponMainIcon;
}

public class LevelInfo
{
    public string description;
    public string sceneName;
    public Sprite levelSplashImage;
}