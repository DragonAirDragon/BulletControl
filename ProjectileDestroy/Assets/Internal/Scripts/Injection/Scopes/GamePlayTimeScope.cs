using System.Collections.Generic;
using DinoFracture;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class GamePlayTimeScope : LifetimeScope
{
    [Title("Configurations And Necessary Objects (Конфигурации и необходимые данные)")]
    [Space]
    [SerializeField] private GameSettings settings;
    [SerializeField] private WeaponAndBulletSettings weaponAndBulletSettings;
    [SerializeField] private LevelObjects levelObjects;

    [SerializeField] private LevelEconomyData levelEconomyData;

    [Title("List To Inject (Список к инъекции)")]
    [Space]
    [SerializeField] private Player player;
    [SerializeField] private SmoothFollowAndRotate cameraFollow;
    [SerializeField] private List<TriggeredObjectController> triggeredObjectControllers = new List<TriggeredObjectController>();

    [Title("Temporary Setting (Временная настройка)")]
    [Space]
    public Weapon weapon;


    protected override void Configure(IContainerBuilder builder)
    {

        // Настройка ввода
        BindInput(builder, settings);
        // Создание сервиса уровня
        BindLevel(builder);
        // Иньъекция в триггерные объекты
        InjectAllTriggeredObjectControllers(builder);
        // Инициализация сервиса эффектов экрана 
        builder.Register<PostProcessingEffectService>(Lifetime.Singleton);

        // Конфиг времени
        builder.RegisterInstance(settings.timeSettings);

        // Создание фэктори для пуль
        BindBulletFactory(builder, weaponAndBulletSettings);

        // Конфиг оружия
        builder.RegisterInstance(weaponAndBulletSettings.weaponAndWeaponSettings[weapon]);

        builder.RegisterComponent(player);
        builder.RegisterComponent(cameraFollow);



        builder.RegisterInstance(levelEconomyData);
        builder.RegisterEntryPoint<LevelScoreService>();
    }

    private void BindBulletFactory(IContainerBuilder builder, WeaponAndBulletSettings weaponAndBulletSettings)
    {
        builder.Register<BulletFactory>(Lifetime.Singleton)
            .WithParameter(weaponAndBulletSettings.bulletCaliberAndBullet);
    }

    private void BindInput(IContainerBuilder builder, GameSettings settings)
    {
        builder.RegisterInstance(settings.inputSettings);
        builder.Register<DesktopInputService>(Lifetime.Singleton).As<IInputService>();
    }

    private void BindLevel(IContainerBuilder builder)
    {
        // Use named registrations to avoid conflicts
        builder.RegisterInstance(levelObjects);
        builder.Register<LevelService>(Lifetime.Singleton);
        builder.RegisterEntryPoint<PauseService>();


    }

    private void InjectAllTriggeredObjectControllers(IContainerBuilder builder)
    {
        foreach (var triggeredObjectController in triggeredObjectControllers)
        {
            builder.RegisterComponent(triggeredObjectController);
        }

    }

}