using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Settings;
using VContainer.Unity;

public class LevelScoreService : IStartable,IDisposable
{
    private LevelEconomyData levelEconomyData;
    private LevelService levelService;
    private BulletFactory bulletFactory;
    private LocalAndCloudDataService localAndCloudDataService;
    
    private float _time = 0f;
    private bool _timerOn = false;
    private GameSessionView gameSessionView;
    private int result;
    
    public LevelScoreService(LevelEconomyData levelEconomyData, LevelService levelService, BulletFactory bulletFactory,  GameSessionView gameSessionView,LocalAndCloudDataService localAndCloudDataService)
    {
        this.levelEconomyData = levelEconomyData;
        this.levelService = levelService;
        this.bulletFactory = bulletFactory;
        this.gameSessionView = gameSessionView;
        this.localAndCloudDataService = localAndCloudDataService;
        
        this.levelService.OnRequiredObjectsDestroyed += Pause;
        this.levelService.OnPaused += Pause;
        this.levelService.OnUnpaused += Continue;
        this.levelService.OnRequiredObjectsDestroyed += CalculateScore;
        this.levelService.OnRequiredObjectsDestroyed += () =>
        {
            this.gameSessionView.allMainTargetsCompleted = true;
            this.gameSessionView.UpdateLevelInfo(levelEconomyData.nameLevel,levelEconomyData.requiredObjectString,levelEconomyData.optionalObjectString,levelEconomyData.triggerObjectString);
        };
        this.levelService.OnOptionalObjectsDestroyed += (() =>
        {
            this.gameSessionView.allOptionalTargetsCompleted = true;
            this.gameSessionView.UpdateLevelInfo(levelEconomyData.nameLevel,levelEconomyData.requiredObjectString,levelEconomyData.optionalObjectString,levelEconomyData.triggerObjectString);
        });
        this.levelService.OnTriggerObjectsDestroyed += () =>
        {
            this.gameSessionView.allTriggerTargetsCompleted = true;
            this.gameSessionView.UpdateLevelInfo(levelEconomyData.nameLevel,levelEconomyData.requiredObjectString,levelEconomyData.optionalObjectString,levelEconomyData.triggerObjectString);
        };
    }

    void IStartable.Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        _time = 0f;
        _timerOn = true;
        //Debug.Log("Timer started.");
        UpdateTimer().Forget();
        gameSessionView.UpdateLevelInfo(levelEconomyData.nameLevel,levelEconomyData.requiredObjectString,levelEconomyData.optionalObjectString,levelEconomyData.triggerObjectString);
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }
    void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        gameSessionView.UpdateLevelInfo(levelEconomyData.nameLevel,levelEconomyData.requiredObjectString,levelEconomyData.optionalObjectString,levelEconomyData.triggerObjectString);
        gameSessionView.UpdateWinUI(levelEconomyData.nameLevel, levelService.GetOptionalCount().currentOptionalObject,levelService.GetOptionalCount().maxOptionalObject, _time,result);
    }

    public void Dispose()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        
    }


    private async UniTaskVoid UpdateTimer()
    {
        while (_timerOn)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            _time += 1f;
        }
    }

    private void Pause()
    {
        _timerOn = false;
    }
    private void Continue()
    {
        _timerOn = true;
        UpdateTimer().Forget();
    }
    public void CalculateScore()
    {
        var optionalObject = levelService.GetOptionalObjectCounts();
        var bullet_cost = bulletFactory.GetCostForAllUsedBullets();

        float optionalRatio = (optionalObject.maxOptional != 0) ?
            (float)optionalObject.currentOptional / optionalObject.maxOptional : 0f;

        float optional_cost = Mathf.Lerp(levelEconomyData.OptionalObjectsCostRange.WorstCost,
            levelEconomyData.OptionalObjectsCostRange.BestCost, optionalRatio);

        float time_cost = Mathf.Lerp(levelEconomyData.TimeCostRange.WorstCost, levelEconomyData.TimeCostRange.BestCost,
                1 - (Mathf.Clamp(_time, levelEconomyData.TimeRange.BestTime, levelEconomyData.TimeRange.WorstTime) / levelEconomyData.TimeRange.WorstTime));

        result = (int)(levelEconomyData.baseLevelCost + optional_cost + time_cost - bullet_cost);
        gameSessionView.SetGameStageWin();
        gameSessionView.UpdateWinUI(levelEconomyData.nameLevel, levelService.GetOptionalCount().currentOptionalObject,levelService.GetOptionalCount().maxOptionalObject, _time,result);
        
        Cursor.lockState = CursorLockMode.None;
        localAndCloudDataService.ChangeCurrentLevel(1);
        gameSessionView.ReturnToMenuBind(result,localAndCloudDataService);
        /*Debug.Log($"Победа\n" +
                  $"Основная награда за уровень {levelEconomyData.baseLevelCost}$ \n" +
                  $"Деньги за опциональные объекты {optional_cost}$ \n" +
                  $"Деньги за время {time_cost}$ \n" +
                  $"Траты на пули {bullet_cost}$ \n" +
                  $"Результат {result}$ \n");
        */
    }
    
}


[Serializable]
public class LevelEconomyData
{
    public string nameLevel;

    public string requiredObjectString;
    public string optionalObjectString;
    public string triggerObjectString;
    
    
    [SerializeField, BoxGroup("Base Cost")]
    public int baseLevelCost = 0;

    [SerializeField, BoxGroup("Time Range")]
    private int bestTime;
    [SerializeField, BoxGroup("Time Range")]
    private int worstTime;

    [SerializeField, BoxGroup("Time Cost Range")]
    private int bestTimeCost;
    [SerializeField, BoxGroup("Time Cost Range")]
    private int worstTimeCost;

    [SerializeField, BoxGroup("Optional Objects Cost Range")]
    private int bestOptionalCost;
    [SerializeField, BoxGroup("Optional Objects Cost Range")]
    private int worstOptionalCost;

    [ShowInInspector, BoxGroup("Time Range")]
    public (int BestTime, int WorstTime) TimeRange
    {
        get => (bestTime, worstTime);
        set
        {
            bestTime = value.BestTime;
            worstTime = value.WorstTime;
        }
    }

    [ShowInInspector, BoxGroup("Time Cost Range")]
    public (int BestCost, int WorstCost) TimeCostRange
    {
        get => (bestTimeCost, worstTimeCost);
        set
        {
            bestTimeCost = value.BestCost;
            worstTimeCost = value.WorstCost;
        }
    }

    [ShowInInspector, BoxGroup("Optional Objects Cost Range")]
    public (int BestCost, int WorstCost) OptionalObjectsCostRange
    {
        get => (bestOptionalCost, worstOptionalCost);
        set
        {
            bestOptionalCost = value.BestCost;
            worstOptionalCost = value.WorstCost;
        }
    }
}