using UnityEngine;
using System.Collections;
#if (UNITY_EDITOR || UNITY_EDITOR_WIN)
using UnityEditor;
#endif

public class SettingAsset : ScriptableObject
{

    /// <summary>
    /// ランキングデータ
    /// </summary>
    public GameSettings settings;

}