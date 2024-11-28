using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class LocalAndCloudDataService
{
    // PermanentData
    private Dictionary<Weapon, WeaponSettings> weaponAndWeaponSettings;
    private List<Weapon> weaponsListOrder;
    private Dictionary<BulletСaliber, Bullet> bulletCaliberAndBullet;
    private Dictionary<int, LevelInfo> numberAndLevelInfo;
    
    //Actions
    public event Action<int> OnMoneyChanged;

    public event Action OnDataUpdated;
    
    
    
    //YandexService
    public LocalAndCloudDataService(Dictionary<Weapon, WeaponSettings> weaponAndWeaponSettings, Dictionary<BulletСaliber, Bullet> bulletCaliberAndBullet,List<Weapon> weaponsListOrder ,Dictionary<int, LevelInfo> numberAndLevelInfo)
    {
        this.weaponAndWeaponSettings = weaponAndWeaponSettings;
        this.bulletCaliberAndBullet = bulletCaliberAndBullet;
        this.weaponsListOrder = weaponsListOrder;
        this.numberAndLevelInfo = numberAndLevelInfo;
        
        //this.yandex = yandex;
        //this.yandex.OnDataLoaded += LoadData;
        YandexGame.GetDataEvent += LoadData;
        YandexGame.PurchaseSuccessEvent += SuccessPurchased;
    }
    
    void SuccessPurchased(string id)
    {
        // Ваш код для обработки покупки. Например:
        if (id == "10")
        {
            ChangeCurrentMoney(99999);
        }
        else if (id == "20")
        {
            SetAdActivity(false);
            OnDataUpdated?.Invoke();
        }
        YandexGame.SaveProgress();
    }
    
    private void LoadData()
    {
        OnDataUpdated?.Invoke();
        OnMoneyChanged?.Invoke(YandexGame.savesData.moneyCount);
    }
    


    #region Money
    public int GetCurrentMoney()
    {
        return YandexGame.savesData.moneyCount;
    }

    public void ChangeCurrentMoney(int value)
    {
        YandexGame.savesData.moneyCount = Mathf.Clamp(YandexGame.savesData.moneyCount + value, 0, 9999);
        YandexGame.SaveProgress();
        OnMoneyChanged?.Invoke(YandexGame.savesData.moneyCount); 
    }

    #endregion

    #region Weapon
    //Equipment
    
    public Weapon GetEquipmentWeapon()
    {
        return YandexGame.savesData.equipmentWeapon;
    }

    public void SetEquipmentWeapon(Weapon newWeapon)
    {
        YandexGame.savesData.equipmentWeapon = newWeapon;
    }
    //Weapon
    public WeaponInfo GetWeaponInfoByWeapon(Weapon weapon)
    {
        return weaponAndWeaponSettings[weapon].weaponInfo;
    }
    
    public Bullet GetBulletParamInfoByWeapon(Weapon weapon)
    {
        BulletСaliber currentCalliber = weaponAndWeaponSettings[weapon].bulletСaliber;
        return bulletCaliberAndBullet[currentCalliber];
    }
 
    
    public bool CheckPurchasedWeapon(Weapon weapon)
    {
        if (YandexGame.savesData.purchasedWeapons.Contains(weapon))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void BuyWeapon(Weapon weapon)
    {
        if (weaponAndWeaponSettings[weapon].weaponInfo.weaponCost > YandexGame.savesData.moneyCount) return;
        SetWeaponPurchasedStatus(weapon);
        ChangeCurrentMoney(-weaponAndWeaponSettings[weapon].weaponInfo.weaponCost);
    }
    
    
    
    public void SetWeaponPurchasedStatus(Weapon weapon)
    {
        if (!YandexGame.savesData.purchasedWeapons.Contains(weapon))
        {
            YandexGame.savesData.purchasedWeapons.Add(weapon);
        }
        YandexGame.SaveProgress();
    }

    public bool CheckLevelAndWeaponRequiredLevel(Weapon weapon)
    {
        return weaponAndWeaponSettings[weapon].weaponInfo.unlockLevel <= YandexGame.savesData.currentLevel;
    }
    
    public List<Weapon> GetWeaponOrder()
    {
        return weaponsListOrder;
    }

    public WeaponSettings GetCurrentWeaponSettings()
    {
        return weaponAndWeaponSettings[YandexGame.savesData.equipmentWeapon];
    }

    

    #endregion

    #region Ad
    // Ad
    public void SetAdActivity(bool value)
    {
        YandexGame.savesData.showAd = value;
        YandexGame.SaveProgress();
    }

    public bool GetAdActivity()
    {
        return YandexGame.savesData.showAd;
    }
    #endregion
    
    #region Level
    // Change Current Level
    
    public void NextLevel()
    {
        YandexGame.savesData.currentLevel = Mathf.Clamp(YandexGame.savesData.currentLevel+1, 0, 44);
        YandexGame.SaveProgress();
    }
    public int GetCurrentLevel()
    {
        return YandexGame.savesData.currentLevel;
    }

    public LevelInfo GetCurrentLevelInfo()
    {
        return numberAndLevelInfo[YandexGame.savesData.currentLevel];
    }
    
    #endregion





}


