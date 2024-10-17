using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelInfo
{
    public string description;
    public string sceneName;
    public Sprite levelSplashImage;
}

[CreateAssetMenu(fileName = "LevelSettings", menuName = "MyGame/Settings")]
public class LevelSettings : ScriptableObject
{
    public Dictionary<int, LevelInfo> numberAndLevelInfo = new Dictionary<int, LevelInfo>();
}
