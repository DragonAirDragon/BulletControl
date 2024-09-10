using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer.Unity;

public class LevelScoreService : IStartable
{
    private LevelEconomyData levelEconomyData;
    private LevelService levelService;
    private BulletFactory bulletFactory;
    private float _time = 0f;
    private bool _timerOn = false;

    public LevelScoreService(LevelEconomyData levelEconomyData, LevelService levelService, BulletFactory bulletFactory)
    {
        this.levelEconomyData = levelEconomyData;
        this.levelService = levelService;
        this.bulletFactory = bulletFactory;
        levelService.OnRequiredObjectsDestroyed += Pause;
        levelService.OnRequiredObjectsDestroyed += () => CalculateScore(true);
    }

    void IStartable.Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        _time = 0f;
        _timerOn = true;
        Debug.Log("Timer started.");
        UpdateTimer().Forget();
    }

    private async UniTaskVoid UpdateTimer()
    {
        while (_timerOn)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            Debug.Log("Time: " + _time);
            _time += 1f;
        }
    }

    private void Pause()
    {
        _timerOn = false;
    }
    public void CalculateScore(bool winOrLose)
    {
        bool resultLevel = winOrLose;
        var optionalObject = levelService.GetOptionalObjectCounts();
        var bullet_cost = bulletFactory.GetCostForAllUsedBullets();

        float optionalRatio = (optionalObject.maxOptional != 0) ?
            (float)optionalObject.currentOptional / optionalObject.maxOptional : 0f;

        float optional_cost = Mathf.Lerp(levelEconomyData.OptionalObjectsCostRange.WorstCost,
            levelEconomyData.OptionalObjectsCostRange.BestCost, optionalRatio);

        float time_cost = Mathf.Lerp(levelEconomyData.TimeCostRange.WorstCost, levelEconomyData.TimeCostRange.BestCost,
                1 - (Mathf.Clamp(_time, levelEconomyData.TimeRange.BestTime, levelEconomyData.TimeRange.WorstTime) / levelEconomyData.TimeRange.WorstTime));

        float result = levelEconomyData.baseLevelCost + optional_cost + time_cost - bullet_cost;

        Debug.Log($"Победа: {resultLevel} \n" +
                  $"Основная награда за уровень {levelEconomyData.baseLevelCost}$ \n" +
                  $"Деньги за опциональные объекты {optional_cost}$ \n" +
                  $"Деньги за время {time_cost}$ \n" +
                  $"Траты на пули {bullet_cost}$ \n" +
                  $"Результат {result}$ \n");
    }


}


[Serializable]
public class LevelEconomyData
{
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