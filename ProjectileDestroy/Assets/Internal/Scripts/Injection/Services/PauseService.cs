using UnityEngine;
using VContainer.Unity;

public class PauseService : ITickable
{
    private LevelService levelService;
    private IInputService inputService;
    private bool isPaused = false;
    public PauseService(LevelService levelService, IInputService inputService)
    {
        this.levelService = levelService;
        this.inputService = inputService;
    }

    void ITickable.Tick()
    {
        if (inputService.GetPause())
        {
            isPaused = !isPaused;
            Debug.Log("Pause: " + isPaused);
            if (isPaused)
            {
                levelService.Pause();
            }
            else levelService.Unpause();
        }
    }
}