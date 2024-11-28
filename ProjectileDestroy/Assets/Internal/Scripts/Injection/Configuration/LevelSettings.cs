using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class LevelInfo
{
    public Sprite levelSplashImage;
}

[CreateAssetMenu(fileName = "LevelSettings", menuName = "MyGame/LevelSettings")]
public class LevelSettings : SerializedScriptableObject
{
    public Dictionary<int, LevelInfo> numberAndLevelInfo = new Dictionary<int, LevelInfo>();
}
