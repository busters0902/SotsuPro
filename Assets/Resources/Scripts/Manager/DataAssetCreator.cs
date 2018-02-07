using UnityEngine;
using System.Collections;

#if (UNITY_EDITOR || UNITY_EDITOR_WIN)
using UnityEditor;

public class DataAssetCreator
{

	[MenuItem("Assets/Create SettingAsset")]
	public static void CreateSettingAsset()
	{
		AssetCreator.CreateAsset<SettingAsset>("Resources");
	}
}

#endif


