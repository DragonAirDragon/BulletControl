using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalAndCloudDataService
{
    // ChangedData
    private int moneyCount = 0;
    private List<Weapon> purchasedWeapons = new List<Weapon>() { Weapon.ColtPython };
    private bool showAd = false;
    private int currentLevel = 0;
    private Weapon currentWeapon;

    // PermanentData
    public Dictionary<Weapon, WeaponSettings> weaponAndWeaponSettings;
    private Dictionary<BulletСaliber, Bullet> bulletCaliberAndBullet;
    private Dictionary<int, LevelInfo> numberAndLevelInfo = new Dictionary<int, LevelInfo>();

    public LocalAndCloudDataService(Dictionary<Weapon, WeaponSettings> weaponAndWeaponSettings, Dictionary<BulletСaliber, Bullet> bulletCaliberAndBullet, Dictionary<int, LevelInfo> numberAndLevelInfo)
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


    #region Money
    public int GetCurrentMoney()
    {
        return moneyCount;
    }

    public void ChangeCurrentMoney(int value)
    {
        moneyCount = Mathf.Clamp(moneyCount + value, 0, 9999);
        SaveOnCloudData();
    }

    #endregion

    #region Weapon
    public WeaponInfo GetWeaponParam(Weapon weapon)
    {
        return weaponAndWeaponSettings[weapon].weaponInfo;
    }
    public Bullet GetBulletParam(Weapon weapon)
    {
        BulletСaliber currentCalliber = weaponAndWeaponSettings[weapon].bulletСaliber;
        return bulletCaliberAndBullet[currentCalliber];
    }

    public bool CheckPurchasedWeapon(Weapon weapon)
    {
        if (purchasedWeapons.Contains(weapon))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public List<Weapon> GetAllWeapons()
    {
        return weaponAndWeaponSettings.Keys.ToList();
    }

    public void SetWeaponPurchasedStatus(Weapon weapon)
    {
        if (!purchasedWeapons.Contains(weapon))
        {
            purchasedWeapons.Add(weapon);
        }
        SaveOnCloudData();
    }

    #endregion

    #region Ad
    // Ad
    public void SetAdActivity(bool value)
    {
        showAd = value;
    }

    public bool GetAdActivity()
    {
        return showAd;
    }
    #endregion


    #region Level
    // Change Current Level
    public void ChangeCurrentLevel(int value)
    {
        currentLevel = Mathf.Clamp(value, 0, 50);
    }
    #endregion





}


