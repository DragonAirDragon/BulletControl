using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class LanguageSetter : MonoBehaviour
{
    public void ForgetLocalize()
    {
        SetLanguage(YandexGame.EnvironmentData.language);
    }
    // ReSharper disable Unity.PerformanceAnalysis
    private void SetLanguage(string languageCode)
    {
        I2.Loc.LocalizationManager.CurrentLanguageCode = languageCode;
    }
}