
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;


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
   
   
   [SerializeField] private Image maneuverabilityPropertyBar;
   
 
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
      SceneManager.LoadScene("Level" + (_localAndCloudDataService.GetCurrentLevel()+1).ToString() + "A");
      
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
      //LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
      _localAndCloudDataService.OnDataUpdated += UpdateAllUI;
   }

   private void OnDestroy()
   {
      //LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
      _localAndCloudDataService.OnDataUpdated -= UpdateAllUI;
   }
   /*
   void OnLocaleChanged(UnityEngine.Localization.Locale locale)
   {
      UpdateWeaponInfoPanelAndImage();
   }
   */
   public void UpdateWeaponInfoPanelAndImage()
   {
      WeaponInfo weaponInfo = _localAndCloudDataService.GetWeaponInfoByWeapon(selectedWeapon);
      Bullet bullet = _localAndCloudDataService.GetBulletParamInfoByWeapon(selectedWeapon);
      splashImage.sprite = weaponInfo.weaponMainIcon;
      weaponName.text = weaponInfo.weaponName;
      ricochetsPropertyValue.text = bullet.CountRicochet.ToString();
      
      float speedTime = (bullet.Speed - bullet.MinSpeed) / (bullet.MaxSpeed - bullet.MinSpeed);
      
     
      costBulletPropertyValue.text = bullet.bulletCost.ToString();
      
      
      ricochetsPropertyBar.fillAmount = (bullet.CountRicochet - bullet.MinCountRicochet) / (bullet.MaxCountRicochet - bullet.MinCountRicochet);
      maneuverabilityPropertyBar.fillAmount = (bullet.Maneuverability-bullet.MinManeuverability) / (bullet.MaxManeuverability-bullet.MinManeuverability);
      speedPropertyBar.fillAmount = speedTime;
      costBulletPropertyBar.fillAmount = (bullet.BulletCost - bullet.MinBulletCost)/ (bullet.MaxBulletCost-bullet.MinBulletCost);
      
     // weaponInfo.weaponCost
     
     //buying
     if (!_localAndCloudDataService.CheckPurchasedWeapon(selectedWeapon))
     {
        actionButton.interactable = true;
        actionButton.colors = actionButtonBuyingColor;
        
        actionButtonText.text = I2.Loc.LocalizationManager.GetTranslation("selectWeaponBuy") + "\n" + weaponInfo.weaponCost+"$";
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
        
        actionButtonText.text = I2.Loc.LocalizationManager.GetTranslation("selectWeaponEquip");
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
       
        actionButtonText.text =  I2.Loc.LocalizationManager.GetTranslation("selectWeaponEquipped");
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
