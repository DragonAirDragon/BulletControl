using System;
using UnityEngine;
using VContainer.Unity;

public class PauseService : ITickable,IDisposable
{
    private LevelService levelService;
    private IInputService inputService;
    private bool isPaused = false;
    private SettingsView settingsViewUI;
    
    public PauseService(LevelService levelService, IInputService inputService, SettingsView settingsViewUI)
    {
        this.levelService = levelService;
        this.inputService = inputService;
        this.settingsViewUI = settingsViewUI;
        settingsViewUI.closeButton.onClick.AddListener(Pause);
    }

    void ITickable.Tick()
    {
        if (inputService.GetPause())
        {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = !isPaused;
        //Debug.Log("Pause: " + isPaused);
        if (isPaused)
        {
            levelService.Pause();
            Cursor.lockState = CursorLockMode.None;
            settingsViewUI.gameObject.SetActive(true);
        }
        else
        {
            levelService.Unpause();
            Cursor.lockState = CursorLockMode.Locked;
            settingsViewUI.gameObject.SetActive(false);
        }
    }

    public void Dispose()
    {
        settingsViewUI.closeButton.onClick.RemoveListener(Pause);
    }
}