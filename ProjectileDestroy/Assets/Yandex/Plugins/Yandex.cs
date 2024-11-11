using System;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Yandex : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GiveMePlayerData();
    
    [DllImport("__Internal")]
    private static extern void RateGame();

    [DllImport("__Internal")]
    private static extern void SaveExtern(string data);
    
    [DllImport("__Internal")]
    private static extern void LoadExtern();
    
    [DllImport("__Internal")]
    private static extern string GetLang();
    
    
    public event Action<ChangedData> OnDataLoaded;

    public async void Start()
    {
        LoadDataFromYandex();

        await InitializeLocalization();
        
        SetLanguage(GetLang());
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
    
    public void LoadDataFromYandex()
    {
        LoadExtern();
    }
    public void SaveDataToYandex(ChangedData data)
    {
        string json = JsonUtility.ToJson(data);
        SaveExtern(json);
    }

    public void DownloadDataFromYandex(string data)
    {
        ChangedData changedData = JsonUtility.FromJson<ChangedData>(data);
        OnDataLoaded?.Invoke(changedData);
    }
}
