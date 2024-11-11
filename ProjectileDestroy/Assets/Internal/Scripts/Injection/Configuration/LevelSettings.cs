using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class LevelInfo
{
    public string descriptionKey;
    public string levelNameKey;
    public Sprite levelSplashImage;
    public string sceneName;
}

[CreateAssetMenu(fileName = "LevelSettings", menuName = "MyGame/LevelSettings")]
public class LevelSettings : SerializedScriptableObject
{
    public Dictionary<int, LevelInfo> numberAndLevelInfo = new Dictionary<int, LevelInfo>();
}
