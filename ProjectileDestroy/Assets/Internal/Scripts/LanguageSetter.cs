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
/*
    private IEnumerator Localize()
    {
        // Ждем завершения инициализации
        yield return StartCoroutine(InitializeLocalization());

        // Устанавливаем язык на основе данных Yandex Game
        string languageCode = YandexGame.EnvironmentData.language;
        Debug.Log($"Попытка установки языка: {languageCode}");

        if (!string.IsNullOrEmpty(languageCode))
        {
            SetLanguage(languageCode);
        }
        else
        {
            Debug.LogWarning("Код языка пустой или не определен.");
        }
    }


    private IEnumerator InitializeLocalization()
    {
       // Ждем завершения инициализации настроек локализации
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            yield return LocalizationSettings.InitializationOperation;
        }

        // Дополнительная проверка наличия таблиц локализации
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale == null)
            {
                Debug.LogError("Одна из локалей недоступна. Проверьте настройки локализации.");
            }
        }

    yield return null;
    }
*/
    // ReSharper disable Unity.PerformanceAnalysis
    private void SetLanguage(string languageCode)
    {
        I2.Loc.LocalizationManager.CurrentLanguageCode = languageCode;
    }
}