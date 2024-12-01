using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using VContainer;
using YG;

public class DonatView : MonoBehaviour
{
    public Button buyMoneyButton;
    public Button buyAdFree;
    public TextMeshProUGUI costOneHundredDollar;
    
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
        //Debug.Log("Покупка денег");
        YandexGame.BuyPayments("10");
    }
    public void BuyAdFree()
    {
        Debug.Log("Покупка рекламы");
        YandexGame.BuyPayments("20");
    }
    
   
    
    private void UpdateUI()
    {
        costOneHundredDollar.text = YandexGame.purchases[1].price;
        if (!localAndCloudDataService.GetAdActivity())
        {
            colorUIImageAnimation.SetBuyingAd();
            buyAdFree.interactable = false;
        }
        else
        {
            colorUIImageAnimation.SetUnbuyingAd();
            buyAdFree.interactable = true;
        }
    }

    public void OnEnable()
    {
        UpdateUI();
    }
}
