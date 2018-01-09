using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// FlashTextを使いやすくするためのデータ
/// </summary>
[System.Serializable]
public class FlashTextObject
{
    public Text text;
    public TextFrash flash;
    public FlashTextObject(Text t, TextFrash f) { text = t; flash = f; }
    public void Set(FlashTextSettings s)
    {
        if (s.name != null) text.gameObject.name = s.name;
        if (s.comment != null) text.text = s.comment;
        if (s.pos != null) text.rectTransform.position = s.pos;
        if (s.pos != null) text.rectTransform.localScale = s.size;
        flash.useFrash = s.useFlash;
    }
}

/// <summary>
/// FlashTextObjectを初期化するデータ
/// </summary>
[System.Serializable]
public class FlashTextSettings
{
    public string name;
    public string comment;
    public Vector3 pos;
    public Vector3 size;
    public bool useFlash;
}