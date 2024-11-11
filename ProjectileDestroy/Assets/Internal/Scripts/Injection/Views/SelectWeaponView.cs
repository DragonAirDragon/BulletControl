using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SelectWeaponView : MonoBehaviour
{
   [Title("Weapon List (Список оружия в UI)")]
   [Space]
   
   [SerializeField] private WeaponElementView[] weaponsElementViewArray = new WeaponElementView[24];
   
   [Title("Weapon Info And Splash Image (Окно информации об оружии и главное изображение)")]
   [Space]
   [SerializeField] private Image splashImage;
   [SerializeField] private TextMeshProUGUI weaponName;
   
   [SerializeField] private TextMeshProUGUI ricochetsPropertyValue;
   [SerializeField] private Image ricochetsPropertyBar;
   
   [SerializeField] private TextMeshProUGUI maneuverabilityPropertyValue;
   [SerializeField] private Image maneuverabilityPropertyBar;
   
   [SerializeField] private TextMeshProUGUI speedPropertyValue;
   [SerializeField] private Image speedPropertyBar;
   
   [SerializeField] private TextMeshProUGUI costBulletPropertyValue;
   [SerializeField] private Image costBulletPropertyBar;
   
   [Title("Action Button")]
   [Space]
   [SerializeField] private Button actionButton;
   
   [SerializeField] private ColorBlock actionButtonBuyingColor; 
   
   [SerializeField] private ColorBlock actionButtonEquippingColor;
   
   [SerializeField] private TextMeshProUGUI actionButtonText;
   
   
   
   
   private LocalAndCloudDataService _localAndCloudDataService;
   private Weapon selectedWeapon;
   
   public Button loadLevelButton;


 
   
   [Inject]
   public void Construct(LocalAndCloudDataService localAndCloudDataService)
   {
      _localAndCloudDataService = localAndCloudDataService;
      loadLevelButton.onClick.AddListener(LoadLevel);
   }

   public void LoadLevel()
   {
      SceneManager.LoadScene(_localAndCloudDataService.GetCurrentLevelInfo().sceneName);
      
   }

   public void OnEnable()
   {
      UpdateAllUI();
   }

   private void UpdateAllUI()
   {
      selectedWeapon = _localAndCloudDataService.GetEquipmentWeapon();
      UpdateWeaponsUIList();
      UpdateWeaponInfoPanelAndImage();
   }

   private void Start()
   {
      LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
      _localAndCloudDataService.OnDataUpdated += UpdateAllUI;
   }

   private void OnDestroy()
   {
      LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
   }
   void OnLocaleChanged(UnityEngine.Localization.Locale locale)
   {
      UpdateWeaponInfoPanelAndImage();
   }

   public void UpdateWeaponInfoPanelAndImage()
   {
      WeaponInfo weaponInfo = _localAndCloudDataService.GetWeaponInfoByWeapon(selectedWeapon);
      Bullet bullet = _localAndCloudDataService.GetBulletParamInfoByWeapon(selectedWeapon);
      splashImage.sprite = weaponInfo.weaponMainIcon;
      weaponName.text = weaponInfo.weaponName;
      ricochetsPropertyValue.text = bullet.CountRicochet.ToString();
      maneuverabilityPropertyValue.text = bullet.Maneuverability.ToString();
      speedPropertyValue.text = bullet.Speed.ToString();
      costBulletPropertyValue.text = bullet.bulletCost.ToString();
      
      ricochetsPropertyBar.fillAmount = bullet.CountRicochet / bullet.MaxCountRicochet;
      maneuverabilityPropertyBar.fillAmount = bullet.Maneuverability / bullet.MaxManeuverability;
      speedPropertyBar.fillAmount =  bullet.Speed/ bullet.MaxSpeed;
      costBulletPropertyBar.fillAmount = bullet.BulletCost/bullet.MaxBulletCost;
      
     // weaponInfo.weaponCost
     
     //buying
     if (!_localAndCloudDataService.CheckPurchasedWeapon(selectedWeapon))
     {
        actionButton.interactable = true;
        actionButton.colors = actionButtonBuyingColor;
        actionButtonText.text = LocalizationSettings.StringDatabase.GetLocalizedString("SelectWeapon", "selectWeaponBuy") + "\n" + weaponInfo.weaponCost+"$";
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() =>
        {
           _localAndCloudDataService.BuyWeapon(selectedWeapon);
           UpdateWeaponInfoPanelAndImage();
        });
        
     }
     //selected
     else if (_localAndCloudDataService.CheckPurchasedWeapon(selectedWeapon) &&
              (selectedWeapon != _localAndCloudDataService.GetEquipmentWeapon()))
     {
        actionButton.interactable = true;
        actionButton.colors = actionButtonEquippingColor;
        actionButtonText.text = LocalizationSettings.StringDatabase.GetLocalizedString("SelectWeapon", "selectWeaponEquip");
        actionButton.onClick.RemoveAllListeners();
        actionButton.onClick.AddListener(() =>
        {
           EquipWeapon(selectedWeapon);
           UpdateWeaponInfoPanelAndImage();
        });
        
        
     }
     //none
     else if(_localAndCloudDataService.CheckPurchasedWeapon(selectedWeapon) && (selectedWeapon==_localAndCloudDataService.GetEquipmentWeapon()))
     {
        actionButton.interactable = false;
        actionButton.colors = actionButtonEquippingColor;
        actionButtonText.text = LocalizationSettings.StringDatabase.GetLocalizedString("SelectWeapon", "selectWeaponEquipped");
        actionButton.onClick.RemoveAllListeners();
     }
     
   }

  
   
   public void UpdateWeaponsUIList()
   {
      int i = 0;
      foreach (var weapon  in _localAndCloudDataService.GetWeaponOrder())
      {
         weaponsElementViewArray[i].SetAvailability(_localAndCloudDataService.GetWeaponInfoByWeapon(weapon).weaponName,_localAndCloudDataService.GetWeaponInfoByWeapon(weapon).weaponIcon,_localAndCloudDataService.CheckLevelAndWeaponRequiredLevel(weapon));
         weaponsElementViewArray[i].button.onClick.AddListener(() =>
         {
            WeaponElementButtonClick(weapon);
         });
         i++;
      }
      
   }

   private void EquipWeapon(Weapon newEquipmnetWeapon)
   {
      _localAndCloudDataService.SetEquipmentWeapon(newEquipmnetWeapon);
   }

  
   
   private void WeaponElementButtonClick(Weapon weapon)
   {
      selectedWeapon = weapon;
      UpdateWeaponInfoPanelAndImage();
   }
   
   
   
   
   
   
   
}
