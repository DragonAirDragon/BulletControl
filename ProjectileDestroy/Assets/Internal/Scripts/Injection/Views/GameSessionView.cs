using TheraBytes.BetterUi;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    public void SetGameStageWin()
    {
        levelWinWindow.SetActive(true);
    }

    public void UpdateWinUI(string levelName, int subSubTargets, int subMainTargetsMax, float time, float reward)
    {
        levelNameTextInWinWindow.text = LocalizationSettings.StringDatabase.GetLocalizedString("Levels", levelName);
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

    public void UpdateLevelInfo(string levelName,string mainTarget="",string optTargets="",string triggerTargets="")
    {
        levelNameText.text = LocalizationSettings.StringDatabase.GetLocalizedString("Levels", levelName);
        string resultTextDescription="";
        
        if(allMainTargetsCompleted)
        {
            resultTextDescription = "<s>" + "- " + LocalizationSettings.StringDatabase.GetLocalizedString("Levels", mainTarget) + "</s>" + "\n";
        }
        else
        {
            resultTextDescription = "- " + LocalizationSettings.StringDatabase.GetLocalizedString("Levels", mainTarget) + "\n";
        }
        
        if(optTargets!="")
        {
            resultTextDescription += allOptionalTargetsCompleted ? "<s>" + "- " + LocalizationSettings.StringDatabase.GetLocalizedString("Levels", optTargets) + "\n" + "</s>" : "- " + LocalizationSettings.StringDatabase.GetLocalizedString("Levels", optTargets) + "\n";
        }
        if(triggerTargets!="")
        {
            resultTextDescription += allTriggerTargetsCompleted ? "<s>" + "- " + LocalizationSettings.StringDatabase.GetLocalizedString("Levels", triggerTargets) + "\n" + "</s>" : "- " + LocalizationSettings.StringDatabase.GetLocalizedString("Levels", triggerTargets) + "\n";
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
       
    }
    
    
    
    
}
