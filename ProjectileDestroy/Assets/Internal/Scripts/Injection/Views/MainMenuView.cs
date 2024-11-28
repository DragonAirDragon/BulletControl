using System;
using Sirenix.OdinInspector;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YG;

public class MainMenuView : MonoBehaviour
{
   [Title("Level UI (Весь UI который касается уровня)")]
   [Space]
   [SerializeField] private Image levelSplashImage;
   [SerializeField] private BetterTextMeshProUGUI nameLevel;
   [SerializeField] private BetterTextMeshProUGUI descriptionLevel;
   [SerializeField] private TextMeshProUGUI[] numbersLevelPanel = new TextMeshProUGUI[3];
   [SerializeField] private Button resetProgress;
   private LocalAndCloudDataService _localAndCloudDataService;
   
   [Inject]
   public void Construct(LocalAndCloudDataService localAndCloudDataService)
   {
      _localAndCloudDataService = localAndCloudDataService;
      _localAndCloudDataService.OnDataUpdated += () =>
      {
         UpdateUI(_localAndCloudDataService.GetCurrentLevel(),
            _localAndCloudDataService.GetCurrentLevelInfo());
      };
   }

   private void Start()
   {
      LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
      resetProgress.onClick.AddListener(YandexGame.ResetSaveProgress);
   }
   
   
   
   void OnLocaleChanged(UnityEngine.Localization.Locale locale)
   {
      UpdateUI(_localAndCloudDataService.GetCurrentLevel(), _localAndCloudDataService.GetCurrentLevelInfo());
   }

   private void OnEnable()
   {
      UpdateUI(_localAndCloudDataService.GetCurrentLevel(), _localAndCloudDataService.GetCurrentLevelInfo());
      
      
   }

   private void OnDestroy()
   {
      LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
      resetProgress.onClick.RemoveListener(YandexGame.ResetSaveProgress);
   }

   public void UpdateUI(int i ,LevelInfo levelInfo)
   {
      levelSplashImage.sprite = levelInfo.levelSplashImage;
      
      
      
      
      var localizedStringNameLevel = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Levels", "Level" + (i+1).ToString()+ "Name");
      
      var localizedStringDescriptionLevel = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("Levels", "Level" + (i+1).ToString()+ "Description");
      
      
      
      localizedStringNameLevel.Completed += handle =>
      {
         if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
         {
            nameLevel.text = handle.Result;
         }
         else
         {
            Debug.LogError($"Не удалось загрузить локализованную строку для ключа: {"Level" + (i+1).ToString()+ "Name"}");
         }
      };
      
      localizedStringDescriptionLevel.Completed += handle =>
      {
         if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
         {
            descriptionLevel.text = handle.Result;
         }
         else
         {
            Debug.LogError($"Не удалось загрузить локализованную строку для ключа: {"Level" + (i+1).ToString()+ "Description"}");
         }
      };
      
      SetCurrentNumbersLevelPanel(i);
      
   }

   private void SetCurrentNumbersLevelPanel(int currentLevelNumber)
   {
      numbersLevelPanel[0].text = currentLevelNumber.ToString();
      numbersLevelPanel[1].text = (currentLevelNumber+1).ToString();
      numbersLevelPanel[2].text = (currentLevelNumber+2).ToString();
   }
}
