using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using YG;

public class LanguageSetter : MonoBehaviour
{
    public async void Start()
    {
        await InitializeLocalization();
        YandexGame.GameReadyAPI();
        SetLanguage(YandexGame.EnvironmentData.language);
    }
    private async UniTask InitializeLocalization()
    {
        // Ждем завершения инициализации локалей
        await LocalizationSettings.InitializationOperation.Task;
    }
    private void SetLanguage(string languageCode)
    {
        // Ищем локаль по коду языка
        Locale locale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == languageCode);

        if (locale != null)
        {
            // Устанавливаем выбранную локаль
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log($"Язык изменен на: {locale.LocaleName}");
        }
        else
        {
            Debug.LogWarning($"Локаль с кодом '{languageCode}' не найдена.");
        }
    }
}
