using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using VContainer.Unity;

public class AudioService 
{
    private float masterVolume = 0.5f;
    private float musicVolume = 0.5f;
    private AudioMixer audioMixer;
    
    public AudioService(AudioMixer audioMixer)
    {
       this.audioMixer = audioMixer;
       SetMasterVolume(masterVolume);
       SetMusicVolume(musicVolume);
      
    }
    
 
    // Метод для получения текущих значений громкости
    public (float MasterVolume, float MusicVolume) GetVolume()
    {
        return (masterVolume, musicVolume);
    }
    

    // Метод для установки masterVolume
    public void SetMasterVolume(float newVolume)
    {
        if (masterVolume != newVolume)
        {
            masterVolume = newVolume;
            float volumeInDecibels = (masterVolume > 0f) ? Mathf.Log10(masterVolume) * 20f : -80f;
            audioMixer.SetFloat("MasterVolume", volumeInDecibels);
        }
    }

    // Метод для установки musicVolume
    public void SetMusicVolume(float newVolume)
    {
        if (musicVolume != newVolume)
        {
            musicVolume = newVolume;
            float volumeInDecibels = (musicVolume > 0f) ? Mathf.Log10(musicVolume) * 20f : -80f;
            audioMixer.SetFloat("MusicVolume", volumeInDecibels);
        }
    }
    
 
}
