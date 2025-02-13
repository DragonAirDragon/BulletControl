using System;
using Sirenix.OdinInspector;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

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
      //LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
      resetProgress.onClick.AddListener(YandexGame.ResetSaveProgress);
   }
   
   
   /*
   void OnLocaleChanged(UnityEngine.Localization.Locale locale)
   {
      UpdateUI(_localAndCloudDataService.GetCurrentLevel(), _localAndCloudDataService.GetCurrentLevelInfo());
   }
   */
   private void OnEnable()
   {
      UpdateUI(_localAndCloudDataService.GetCurrentLevel(), _localAndCloudDataService.GetCurrentLevelInfo());
      
      
   }

   private void OnDestroy()
   {
      //LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
      resetProgress.onClick.RemoveListener(YandexGame.ResetSaveProgress);
   }

   public void UpdateUI(int i ,LevelInfo levelInfo)
   {
      levelSplashImage.sprite = levelInfo.levelSplashImage;
      
      
      
      
      nameLevel.text = I2.Loc.LocalizationManager.GetTranslation("Level" + (i+1).ToString()+ "Name");
      
      descriptionLevel.text = I2.Loc.LocalizationManager.GetTranslation("Level" + (i+1).ToString()+ "Description");
      
      SetCurrentNumbersLevelPanel(i);
      
   }

   private void SetCurrentNumbersLevelPanel(int currentLevelNumber)
   {
      numbersLevelPanel[0].text = (currentLevelNumber+1).ToString();
      numbersLevelPanel[1].text = (currentLevelNumber+2).ToString();
      numbersLevelPanel[2].text = (currentLevelNumber+3).ToString();
   }
}
