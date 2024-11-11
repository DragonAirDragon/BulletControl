
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class MainMenuTimeScope :  LifetimeScope
{
    [Title("List To Inject (Список к инъекции)")]
    [SerializeField] private MainMenuView _mainMenuView;
    [SerializeField] private SelectWeaponView _selectWeaponView;
    [SerializeField] private MoneyView _moneyView;
    [SerializeField] private MainMenuNavigate _mainMenuNavigate;
    [SerializeField] private DonatView _donatView;
    
   
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(_mainMenuView);
        builder.RegisterComponent(_selectWeaponView);
        builder.RegisterComponent(_moneyView);
        builder.RegisterComponent(_donatView);
        builder.RegisterComponent(_mainMenuNavigate);
    }
}

