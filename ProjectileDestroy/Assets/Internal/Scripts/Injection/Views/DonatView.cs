using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using VContainer;

public class DonatView : MonoBehaviour
{
    public Button buyMoneyButton;
    public Button buyAdFree;
    
    public ColorUIImageAnimation colorUIImageAnimation;
    private LocalAndCloudDataService localAndCloudDataService;
    
    [Inject]
    public void Construct(LocalAndCloudDataService localAndCloudDataService)
    {
        this.localAndCloudDataService = localAndCloudDataService;
    }

    public void Start()
    {
        localAndCloudDataService.OnDataUpdated += UpdateUI;
    }
    public void BuyMoney()
    {
        // добавить хуету для яндекса
        localAndCloudDataService.ChangeCurrentMoney(99999);
    }

    public void BuyAdFree()
    {
        localAndCloudDataService.SetAdActivity(false);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (!localAndCloudDataService.GetAdActivity()) colorUIImageAnimation.SetBuyingAd();
    }

    public void OnEnable()
    {
        UpdateUI();
    }
}
