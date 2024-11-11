using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class SettingsView : MonoBehaviour
{
    public Button closeButton;
    public Scrollbar masterVolumeScrollbar;
    public Scrollbar musicVolumeScrollbar;
    
    private AudioService audioService;

    [Inject]
    public void Construct(AudioService audioService)
    {
        this.audioService = audioService;
        masterVolumeScrollbar.onValueChanged.AddListener(audioService.SetMasterVolume);
        musicVolumeScrollbar.onValueChanged.AddListener(audioService.SetMusicVolume);
    }

    public void OpenSettings()
    {
        gameObject.SetActive(true);
        var volume = audioService.GetVolume();
        masterVolumeScrollbar.value = volume.MasterVolume;
        musicVolumeScrollbar.value = volume.MusicVolume;
    }

    public void CloseSettings() { gameObject.SetActive(false); }
}
