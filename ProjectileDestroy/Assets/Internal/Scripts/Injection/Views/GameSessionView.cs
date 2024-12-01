using System;
using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class GameSessionView : MonoBehaviour
{
    [SerializeField] private GameObject bulletCountGameObject;
    [SerializeField] private GameObject bulletRicochetCountGameObject;
    
    [SerializeField] private TextMeshProUGUI bulletCountText;
    [SerializeField] private TextMeshProUGUI bulletRicochetCountText;

    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private BetterTextMeshProUGUI levelDescriptionText;
    public bool allMainTargetsCompleted;
    public bool allOptionalTargetsCompleted;
    public bool allTriggerTargetsCompleted;

    [SerializeField] private GameObject levelWinWindow;
    
    [SerializeField] private TextMeshProUGUI levelNameTextInWinWindow;
    [SerializeField] private TextMeshProUGUI subMainTargetsText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI rewardText;
    
    public Button returnToMenuButton;
    public Button doubleRewardButton;


   
    public void SetGameStageBullet()
    {
        bulletCountGameObject?.SetActive(false);
        bulletRicochetCountGameObject?.SetActive(true);
    }
    public void SetGameStagePlayer()
    {
        bulletCountGameObject?.SetActive(true);
        bulletRicochetCountGameObject?.SetActive(false);
    }

    public void DoubleRewardSubscribe()
    {
        YandexGame.RewVideoShow(0);
    }
    

    public void SetGameStageWin()
    {
        levelWinWindow.SetActive(true);
    }

    public void UpdateWinUI(int levelNumber, int subSubTargets, int subMainTargetsMax, float time, float reward)
    {
        levelNameTextInWinWindow.text = I2.Loc.LocalizationManager.GetTranslation("Level" + levelNumber.ToString() + "Name");
        
        
        subMainTargetsText.text = subSubTargets.ToString() + "/" + subMainTargetsMax.ToString();
        timeText.text = time.ToString() + " sec";
        rewardText.text = reward.ToString() + "$";
    }


    public void UpdateRicochetUI(int newValueRicochet)
    {
        bulletRicochetCountText.text = newValueRicochet.ToString();
    }
    public void UpdateBulletsUI(int newValueBullets)
    {
        bulletCountText.text = newValueBullets.ToString();
    }

    public void UpdateLevelInfo(int levelNumber, bool optTargetsExist,  bool trgTargetsExist )
    {
        
        levelNameText.text = I2.Loc.LocalizationManager.GetTranslation("Level" + levelNumber.ToString() + "Name");
        string resultTextDescription="";
        string mainTarget;
        
        mainTarget = "Level" + levelNumber + "Req";
        
        if(allMainTargetsCompleted)
        {
            resultTextDescription = "<s>" + "- " + I2.Loc.LocalizationManager.GetTranslation(mainTarget) + "</s>" + "\n";
        }
        else
        {
            resultTextDescription = "- " + I2.Loc.LocalizationManager.GetTranslation(mainTarget) + "\n";
        }
        
        
        
        if(optTargetsExist)
        {
            string optTargets = "Level" + levelNumber + "Opt";
            resultTextDescription += allOptionalTargetsCompleted ? "<s>" + "- " + I2.Loc.LocalizationManager.GetTranslation(optTargets) + "\n" + "</s>" : "- " + I2.Loc.LocalizationManager.GetTranslation(optTargets) + "\n";
        }
        
        if(trgTargetsExist)
        {
            string triggerTargets = "Level" + levelNumber + "Trg";
            resultTextDescription += allTriggerTargetsCompleted ? "<s>" + "- " + I2.Loc.LocalizationManager.GetTranslation(triggerTargets) + "\n" + "</s>" : "- " + I2.Loc.LocalizationManager.GetTranslation(triggerTargets) + "\n";
        }
        
        levelDescriptionText.text = resultTextDescription; 
    }


    public void ReturnToMenuBind(int resultMoney, LocalAndCloudDataService localAndCloudDataService)
    {
        returnToMenuButton.onClick.RemoveAllListeners();
        returnToMenuButton.onClick.AddListener(() =>
        {
            localAndCloudDataService.ChangeCurrentMoney(resultMoney);
            SceneManager.LoadScene("MainMenu");
        });
        doubleRewardButton.onClick.AddListener(DoubleRewardSubscribe);
       
    }

    public void SetActiveDoubleRewardButton(bool value)
    {
        doubleRewardButton.interactable = value;
    }
    
    
    
}
