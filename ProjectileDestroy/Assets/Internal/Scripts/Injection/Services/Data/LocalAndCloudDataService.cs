using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LocalAndCloudDataService
{
    //Changed Data
    private ChangedData _changedData = new ChangedData();
    
    // PermanentData
    private Dictionary<Weapon, WeaponSettings> weaponAndWeaponSettings;
    private List<Weapon> weaponsListOrder;
    private Dictionary<BulletСaliber, Bullet> bulletCaliberAndBullet;
    private Dictionary<int, LevelInfo> numberAndLevelInfo;
    
    //Service
    private Yandex yandex;
    
    //Actions
    public event Action<int> OnMoneyChanged;

    public event Action OnDataUpdated;
    
    
    
    //YandexService
    public LocalAndCloudDataService(Dictionary<Weapon, WeaponSettings> weaponAndWeaponSettings, Dictionary<BulletСaliber, Bullet> bulletCaliberAndBullet,List<Weapon> weaponsListOrder ,Dictionary<int, LevelInfo> numberAndLevelInfo,Yandex yandex)
    {
        this.weaponAndWeaponSettings = weaponAndWeaponSettings;
        this.bulletCaliberAndBullet = bulletCaliberAndBullet;
        this.weaponsListOrder = weaponsListOrder;
        this.numberAndLevelInfo = numberAndLevelInfo;
        
        this.yandex = yandex;
        this.yandex.OnDataLoaded += LoadData;
    }

    private void SaveOnCloudData()
    {
        yandex.SaveDataToYandex(_changedData);
    }
    private void LoadFromCloudData()
    {
        yandex.LoadDataFromYandex();
    }

    private void LoadData(ChangedData changedData)
    {
        if(changedData.purchasedWeapons != null && changedData.purchasedWeapons.Count > 0 && changedData.moneyCount != 0)
        {
            _changedData = changedData;

            Debug.Log("Основные данные: \n" + _changedData.moneyCount.ToString() + "  " +
                      _changedData.currentLevel.ToString() + "  " + _changedData.equipmentWeapon.ToString() + "  " +
                      _changedData.purchasedWeapons.ToString() + "  " + _changedData.showAd.ToString());
            OnDataUpdated?.Invoke();
            OnMoneyChanged?.Invoke(_changedData.moneyCount);
        }
        
    }
    


    #region Money
    public int GetCurrentMoney()
    {
        return _changedData.moneyCount;
    }

    public void ChangeCurrentMoney(int value)
    {
        _changedData.moneyCount = Mathf.Clamp(_changedData.moneyCount + value, 0, 9999);
        SaveOnCloudData();
        OnMoneyChanged?.Invoke(_changedData.moneyCount); 
    }

    #endregion

    #region Weapon
    //Equipment
    
    public Weapon GetEquipmentWeapon()
    {
        return _changedData.equipmentWeapon;
    }

    public void SetEquipmentWeapon(Weapon newWeapon)
    {
        _changedData.equipmentWeapon = newWeapon;
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
        if (_changedData.purchasedWeapons.Contains(weapon))
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
        if (weaponAndWeaponSettings[weapon].weaponInfo.weaponCost > _changedData.moneyCount) return;
        SetWeaponPurchasedStatus(weapon);
        ChangeCurrentMoney(-weaponAndWeaponSettings[weapon].weaponInfo.weaponCost);
    }
    
    
    
    public void SetWeaponPurchasedStatus(Weapon weapon)
    {
        if (!_changedData.purchasedWeapons.Contains(weapon))
        {
            _changedData.purchasedWeapons.Add(weapon);
        }
        SaveOnCloudData();
    }

    public bool CheckLevelAndWeaponRequiredLevel(Weapon weapon)
    {
        return weaponAndWeaponSettings[weapon].weaponInfo.unlockLevel <= _changedData.currentLevel;
    }
    
    public List<Weapon> GetWeaponOrder()
    {
        return weaponsListOrder;
    }

    public WeaponSettings GetCurrentWeaponSettings()
    {
        return weaponAndWeaponSettings[_changedData.equipmentWeapon];
    }

    

    #endregion

    #region Ad
    // Ad
    public void SetAdActivity(bool value)
    {
        _changedData.showAd = value;
        SaveOnCloudData();
    }

    public bool GetAdActivity()
    {
        return _changedData.showAd;
    }
    #endregion
    
    #region Level
    // Change Current Level
    public void ChangeCurrentLevel(int value)
    {
        _changedData.currentLevel = Mathf.Clamp(value, 0, 50);
        SaveOnCloudData();
    }
    public int GetCurrentLevel()
    {
        return _changedData.currentLevel;
    }

    public LevelInfo GetCurrentLevelInfo()
    {
        return numberAndLevelInfo[_changedData.currentLevel];
    }
    
    #endregion





}


