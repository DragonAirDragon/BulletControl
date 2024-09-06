using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using VContainer.Unity;

public class LevelScoreService : ITickable
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
        Initialize();
    }

    void ITickable.Tick()
    {
        if (_timerOn)
        {
            _time += Time.deltaTime;
        }
    }
    private void Initialize()
    {
        _time = 0f;
        _timerOn = true;
    }

    private void Pause()
    {
        _timerOn = false;
    }
    private void Continue()
    {
        _timerOn = true;
    }
    private void ResetTimer()
    {
        _time = 0f;
        _timerOn = false;
    }
    public void CalculateScore(bool winOrLose)
    {
        bool resultLevel = winOrLose;
        var optionalObject = levelService.GetOptionalObjectCounts();
        var bulletCount = bulletFactory.GetCountUsedBullets();
        var optional_cost = Mathf.Lerp(levelEconomyData.optionalObjectsCostRange.WorstCost, levelEconomyData.optionalObjectsCostRange.BestCost, optionalObject.currentOptional / optionalObject.maxOptional);
        var time_cost = Mathf.Lerp(levelEconomyData.timeCostRange.WorstCost, levelEconomyData.timeCostRange.BestCost, 1 - (Mathf.Clamp(_time, levelEconomyData.timeRange.BestTime, levelEconomyData.timeRange.WorstTime) / levelEconomyData.timeRange.WorstTime));
        //var bullet_cost -=  

    }
}


[Serializable]
public class LevelEconomyData
{
    public int baseLevelCost = 0;
    public (int BestTime, int WorstTime) timeRange { get; set; }
    public (int BestCost, int WorstCost) timeCostRange { get; set; }
    public (int BestCost, int WorstCost) bulletsSpentCostRange { get; set; }
    public (int BestCost, int WorstCost) optionalObjectsCostRange { get; set; }


}