using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using YG;

public class MainMenuNavigate : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject selectWeaponUI;
    public GameObject donatViewUI;
    private SettingsView settingsViewUI;
    
    [Inject]
    public void Construct(SettingsView settingsViewUI)
    {
        this.settingsViewUI = settingsViewUI;
    }

    private void Start()
    {
        if(YandexGame.savesData.showAd) YandexGame.FullscreenShow();
    }

    public void OpenSelectWeaponUI()
    {
        selectWeaponUI.SetActive(true);
        mainMenuUI.SetActive(false);
        donatViewUI.SetActive(false);
    }

    public void OpenMainMenuUI()
    {
        mainMenuUI.SetActive(true);
        selectWeaponUI.SetActive(false);
        donatViewUI.SetActive(false);
    }

    public void OpenDonatViewUI()
    {
        donatViewUI.SetActive(true);
        selectWeaponUI.SetActive(false);
        mainMenuUI.SetActive(false);
    }

    public void OpenSettingsUI()
    {
        settingsViewUI.OpenSettings();
    }

  
    
    
}
