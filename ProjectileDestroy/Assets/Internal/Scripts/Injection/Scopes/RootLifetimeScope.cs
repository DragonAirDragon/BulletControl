using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] private WeaponAndBulletSettings _weaponAndBulletSettings;
    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private SettingsView _settingsViewPrefab;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Yandex _yandex;
    
    [FormerlySerializedAs("lifetimeScopePrefab")] public GamePlayTimeScope playTimeScopePrefab;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_weaponAndBulletSettings.weaponAndWeaponSettings);
        builder.RegisterInstance(_weaponAndBulletSettings.bulletCaliberAndBullet);
        builder.RegisterInstance(_weaponAndBulletSettings.weaponsListOrder);
        builder.RegisterInstance(_levelSettings.numberAndLevelInfo);
        builder.Register<AudioService>(Lifetime.Singleton).WithParameter(_mixer);
        
        builder.RegisterComponentInNewPrefab(_yandex, Lifetime.Singleton)
            .DontDestroyOnLoad();
        
        builder.RegisterComponentInNewPrefab(_settingsViewPrefab, Lifetime.Singleton)
            .DontDestroyOnLoad();
        builder.Register<LocalAndCloudDataService>(Lifetime.Singleton);
        
        // Принудительно разрешаем AudioService при построении контейнера
        builder.RegisterBuildCallback(container =>
        {
            container.Resolve<AudioService>();
        });
    }
}
