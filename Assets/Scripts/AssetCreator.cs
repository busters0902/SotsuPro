using UnityEngine;
using System.Collections;
#if (UNITY_EDITOR || UNITY_EDITOR_WIN)
using UnityEditor;
#endif

#if (UNITY_EDITOR || UNITY_EDITOR_WIN)
public class AssetCreator
{
    /// <summary>
    /// 指定クラスのAsset生成
    /// </summary>
    public static void CreateAsset<Type>(string path_ = null) where Type : ScriptableObject
    {
        if (path_ == null) path_ = "";
        else path_ += "/";

        Type item = ScriptableObject.CreateInstance<Type>();

        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/" + path_ + typeof(Type) + ".asset");

        AssetDatabase.CreateAsset(item, path);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = item;
    }

    public static void SaveAsset<Type>(Type item, string path_ ) where Type : ScriptableObject
    { 

        //Type item = ScriptableObject.CreateInstance<Type>();

        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/" + path_ + ".asset");

        //ファイルがない場合にする
        //System.IO.File.Delete(path);
        AssetDatabase.DeleteAsset(path);
        //AssetDatabase.CreateAsset(item, path);
        //AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = item;
    }

}


#endif