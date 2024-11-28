using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Settings;
using VContainer.Unity;
using YG;

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
            this.gameSessionView.UpdateLevelInfo(levelEconomyData.levelNumber,levelEconomyData.optTargetsExist,levelEconomyData.trgTargetsExist);
        };
        this.levelService.OnOptionalObjectsDestroyed += (() =>
        {
            this.gameSessionView.allOptionalTargetsCompleted = true;
            this.gameSessionView.UpdateLevelInfo(levelEconomyData.levelNumber,levelEconomyData.optTargetsExist,levelEconomyData.trgTargetsExist);
        });
        this.levelService.OnTriggerObjectsDestroyed += () =>
        {
            this.gameSessionView.allTriggerTargetsCompleted = true;
            this.gameSessionView.UpdateLevelInfo(levelEconomyData.levelNumber,levelEconomyData.optTargetsExist,levelEconomyData.trgTargetsExist);
        };
    }

    void IStartable.Start()
    {
        Initialize();
        YandexGame.GameplayStart();
        YandexGame.RewardVideoEvent += DoubleRewardSuccess;
        
    }
    private void Initialize()
    {
        _time = 0f;
        _timerOn = true;
        //Debug.Log("Timer started.");
        UpdateTimer().Forget();
        gameSessionView.UpdateLevelInfo(levelEconomyData.levelNumber,levelEconomyData.optTargetsExist,levelEconomyData.trgTargetsExist);
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }
    void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        gameSessionView.UpdateLevelInfo(levelEconomyData.levelNumber,levelEconomyData.optTargetsExist,levelEconomyData.trgTargetsExist);
        gameSessionView.UpdateWinUI(levelEconomyData.levelNumber, levelService.GetOptionalCount().currentOptionalObject,levelService.GetOptionalCount().maxOptionalObject, _time,result);
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
        var bullet_cost = Mathf.Clamp(bulletFactory.GetCostForAllUsedBullets(),0,levelEconomyData.bestBulletCost);
        Debug.Log(bullet_cost);
        float optionalRatio = (optionalObject.maxOptional != 0) ?
            (float)optionalObject.currentOptional / optionalObject.maxOptional : 0f;

        float optional_cost = Mathf.Lerp(levelEconomyData.OptionalObjectsCostRange.WorstCost,
            levelEconomyData.OptionalObjectsCostRange.BestCost, optionalRatio);

        float time_cost = Mathf.Lerp(levelEconomyData.TimeCostRange.WorstCost, levelEconomyData.TimeCostRange.BestCost,
                1 - (Mathf.Clamp(_time, levelEconomyData.TimeRange.BestTime, levelEconomyData.TimeRange.WorstTime) / levelEconomyData.TimeRange.WorstTime));

        result = (int)(levelEconomyData.baseLevelCost + optional_cost + time_cost - bullet_cost);
        gameSessionView.SetGameStageWin();
        gameSessionView.UpdateWinUI(levelEconomyData.levelNumber, levelService.GetOptionalCount().currentOptionalObject,levelService.GetOptionalCount().maxOptionalObject, _time,result);
        levelService.bullet.DestroyBullet();
        Cursor.lockState = CursorLockMode.None;
        localAndCloudDataService.NextLevel();
        gameSessionView.ReturnToMenuBind(result,localAndCloudDataService);
        YandexGame.GameplayStop();
        /*Debug.Log($"Победа\n" +
                  $"Основная награда за уровень {levelEconomyData.baseLevelCost}$ \n" +
                  $"Деньги за опциональные объекты {optional_cost}$ \n" +
                  $"Деньги за время {time_cost}$ \n" +
                  $"Траты на пули {bullet_cost}$ \n" +
                  $"Результат {result}$ \n");
        */
        
    }

    private void DoubleRewardSuccess(int i)
    {
        if(i == 0)
        {
            result *= 2;
            gameSessionView.SetActiveDoubleRewardButton(false);
            gameSessionView.UpdateWinUI(levelEconomyData.levelNumber, levelService.GetOptionalCount().currentOptionalObject,levelService.GetOptionalCount().maxOptionalObject, _time,result);
            gameSessionView.ReturnToMenuBind(result,localAndCloudDataService);
        }
    }
}





[Serializable]
public class LevelEconomyData
{
    public int levelNumber;
    public bool optTargetsExist;
    public bool trgTargetsExist;
    
    
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
    
    public int bestBulletCost;
    

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