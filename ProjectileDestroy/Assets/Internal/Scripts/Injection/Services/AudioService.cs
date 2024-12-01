using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Audio;
using YG;
using Cysharp.Threading.Tasks;

public class AudioService
{
    private AudioMixer audioMixer;
    
    private CancellationTokenSource saveCancellationTokenSource;
    private float saveDelay = 1f; // Задержка перед сохранением в секундах

    public AudioService(AudioMixer audioMixer)
    {
        this.audioMixer = audioMixer;
        YandexGame.GetDataEvent += LoadAudioData;
    }

    // Load data from Yandex
    private void LoadAudioData()
    {
        SetMasterValueToMixer(YandexGame.savesData.masterVolume);
        SetMusicValueToMixer(YandexGame.savesData.musicVolume);
    }

    // Метод для получения текущих значений громкости
    public (float MasterVolume, float MusicVolume) GetVolume()
    {
        return (YandexGame.savesData.masterVolume, YandexGame.savesData.musicVolume);
    }

    // Метод для установки masterVolume
    public void SetMasterVolume(float newVolume)
    {
        if (YandexGame.savesData.masterVolume != newVolume)
        {
            YandexGame.savesData.masterVolume = newVolume;
            SetMasterValueToMixer(YandexGame.savesData.masterVolume);
            StartSaveTask();
        }
    }
    
    // Метод для установки musicVolume
    public void SetMusicVolume(float newVolume)
    {
        if (YandexGame.savesData.musicVolume != newVolume)
        {
            YandexGame.savesData.musicVolume = newVolume;
            SetMusicValueToMixer(YandexGame.savesData.musicVolume);
            StartSaveTask();
        }
    }
    
    
    public void SetMasterValueToMixer(float masterVolume)
    {
        float volumeInDecibels = (masterVolume > 0f) ? Mathf.Log10(masterVolume) * 20f : -80f;
        audioMixer.SetFloat("MasterVolume", volumeInDecibels);
    }
    public void SetMusicValueToMixer(float musicVolume)
    {
        float volumeInDecibels = (musicVolume > 0f) ? Mathf.Log10(musicVolume) * 20f : -80f;
        audioMixer.SetFloat("MusicVolume", volumeInDecibels);
    }
    private void StartSaveTask()
    {
        saveCancellationTokenSource?.Cancel();
        saveCancellationTokenSource = new CancellationTokenSource();
        SaveDataWithDelay(saveCancellationTokenSource.Token).Forget();
    }

    private async UniTaskVoid SaveDataWithDelay(CancellationToken cancellationToken)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(saveDelay), cancellationToken: cancellationToken);
        //Debug.Log("Saving audio data");
        
        YandexGame.SaveProgress();
    }
}
