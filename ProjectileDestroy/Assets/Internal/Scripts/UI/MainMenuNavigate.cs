using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

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
