using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScoreService
{
    private LevelEconomyData levelEconomyData;

    public LevelScoreService(LevelEconomyData levelEconomyData)
    {
        this.levelEconomyData = levelEconomyData;
    }


}


[Serializable]
public class LevelEconomyData
{
    public (int BestTime, int WorstTime) timeRange { get; set; }
    public (int BestCost, int WorstCost) timeCostRange { get; set; }
    public (int BestCount, int WorstCount) countBulletsSpent { get; set; }
    public (int BestCost, int WorstCost) bulletsSpentCostRange { get; set; }
    public (int BestCost, int WorstCost) optionalObjectsCostRange { get; set; }
}