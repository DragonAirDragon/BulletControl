using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class InputSettings
{
    public float defaultBulletManeuverability = 10f;
    public float sensitivityPlayerCamera;
}
[Serializable]
public class TimeSettings
{
    public float timeDilationMultiply;
}

[CreateAssetMenu(fileName = "GameSettings", menuName = "MyGame/Settings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    public InputSettings inputSettings;

    [SerializeField]
    public TimeSettings timeSettings;
}
