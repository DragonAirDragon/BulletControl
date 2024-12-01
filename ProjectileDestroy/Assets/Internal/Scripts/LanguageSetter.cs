using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YG;

public class LanguageSetter : MonoBehaviour
{
    public void ForgetLocalize()
    {
        StartCoroutine(Localize());
    }

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
    }

    private void SetLanguage(string languageCode)
    {
        // Поиск локали по коду языка
        Locale locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == languageCode);

        if (locale != null)
        {
            // Установка выбранной локали
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log($"Язык успешно изменен на: {locale.LocaleName}");
        }
        else
        {
            Debug.LogWarning($"Локаль с кодом '{languageCode}' не найдена.");
        }
    }
}