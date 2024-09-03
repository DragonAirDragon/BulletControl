using System;
using FronkonGames.SpiceUp.Speedlines;
using VContainer.Unity;
public class PostProcessingEffectService : IDisposable
{
    Speedlines.Settings settings;
    public void SetSpeedEffect(bool enabled)
    {
        settings = Speedlines.GetSettings();
        settings.strength = enabled ? 0.7f : 0f;
    }

    public void DisableAllEffects()
    {
        SetSpeedEffect(false);
    }
    public void Dispose()
    {
        DisableAllEffects();
    }
}


