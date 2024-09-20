using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum Weapon
{
    // Pistols
    ColtPython,
    Glock,
    Tec9,
    DesertEagle,
    Colt1911,
    Beretta,
    // SMG
    MicroUzi,
    MP5,
    MP7,
    P90,
    Bizon,
    UMP,
    // Assault Rifles
    AK47,
    AUG,
    G36C,
    M4A4,
    M16,
    SCAR,
    // Sniper Rifles
    AWP,
    Dragunov,
    M24,
    BarretM82,
    Winchester,
    VSSVintorez
}
public enum BulletСaliber
{
    _9x19mmParabellum,
    _45ACP,
    _357Magnum,
    _50AE,
    _9x18mmMakarov,
    _5dot7x28mm,
    _4dot6x30mm,
    _5dot56x45mmNATO,
    _7dot62x39mm,
    _7dot62x51mmNATO,
    _9x39mm,
    _7dot62x54mmR,
    _dot30_06Springfield,
    _338LapuaMagnum,
    _50BMG
}


[Serializable]
public class WeaponInfo
{
    public string weaponName;
    public string weaponCost;
    public Sprite weaponIcon;
    public Sprite weaponMainIcon;
}
[Serializable]
public class WeaponSettings
{

    public WeaponAnimationSettings weaponAnimationSettings;
    [Title("BulletCaliber (Пуля которая подходит для оружия)")][Space] public BulletСaliber bulletСaliber; //+
    public WeaponInfo weaponInfo;
}
[Serializable]
public class WeaponAnimationSettings
{
    [Title("Idle Part (Часть анимации в покое)")]
    [Space]
    public float timeIdleAnimation = 3f;
    public float yDifferent = 0.02f;
    [Title("Aiming (Прицеливание)")]
    [Space]
    public Vector3 aimPosition = new Vector3(0f, -0.2f, 0.25f);
    public Vector3 idlePosition = new Vector3(0.277f, -0.271f, 0.393f);
    public Vector3 aimRotation;
    public Vector3 idleRotation = new Vector3(0f, -5f, 0f);
    public float aimTransitionTime = 0.2f;
    [Title("Shoot (Стрельба)")]
    [Space]
    public Vector3 recoilOffset = new Vector3(0f, 0f, -0.2f);
    public Vector3 recoilRotation = new Vector3(-15f, 0f, 0f);
    public float recoilDuration = 0.1f;
    public float returnDuration = 0.5f;
    [Title("Model (Моделька оружия)")]
    [Space]
    public WeaponAnimator weaponPrefab;
}



[CreateAssetMenu(fileName = "WeaponSettings", menuName = "MyGame/WeaponSettings")]
public class WeaponAndBulletSettings : SerializedScriptableObject
{
    public Dictionary<Weapon, WeaponSettings> weaponAndWeaponSettings = new Dictionary<Weapon, WeaponSettings>();
    public Dictionary<BulletСaliber, Bullet> bulletCaliberAndBullet = new Dictionary<BulletСaliber, Bullet>();
}



