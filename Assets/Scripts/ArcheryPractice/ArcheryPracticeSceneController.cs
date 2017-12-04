using System.Collections;
using System.Collections.Generic;
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
    public FlashTextObject(Text t, TextFrash f){ text = t;  flash = f; }
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

//コルーチンでゲーム進行を管理
public class ArcheryPracticeSceneController : MonoBehaviour
{

    [SerializeField]
    VRArcheryController3 archeryController;

    [SerializeField]
    int shotTimesLimit;

    public bool IsGameEnd;

    //ゲーム進行用のテロップ
    FlashTextObject flashText;

    [SerializeField]
    FlashTextSettings flashTextSettings;

    //ゲーム進行用のテロップ
    [SerializeField]
    Text timesText;

    //テキストマネージャー用
    [SerializeField]
    Canvas canvas;

    [SerializeField]
    ScoreTotal scoreTortal;
    [SerializeField]
    GameObject head;



    void Start ()
    {
        StartCoroutine(Setup());
        
	}

    IEnumerator Setup()
    {
        
        FadeControl.Instance.SetGemeobject(head);



        Debug.Log("SceneController.Setup");

        yield return null;

        TextManager.Instance.SetCanvas(canvas);
        TextManager.Instance.SetPrefab((GameObject)Resources.Load("Model/Prefabs/Text"));

        flashText = CreateGameTelop();
        flashText.Set(flashTextSettings);
        flashText.text.text = "初期化";

        //フェードアウト
        FadeControl.Instance.FadeIn(3, 1);

        yield return StartCoroutine(GameMain());
    }

    IEnumerator GameMain()
    {
        Debug.Log("SceneController.GameMain");

        

        yield return null;

        while(true)
        {
            yield return StartCoroutine(StartGame());

            if (IsGameEnd) break;

            yield return StartCoroutine(PlayGame());

            if (IsGameEnd) break;

            yield return StartCoroutine(ShotResult());

            if (IsGameEnd) break;

            Reset();
        }

        Debug.Log("SceneController.GameMain End");

    }

    IEnumerator StartGame()
    {
        Debug.Log("SceneController.StartGame");

        yield return null;

        Debug.Log("ゲームを開始します。右トリガーを引いてください");

        flashText.text.text = "ゲームを開始します。右トリガーを引いてください";
        flashText.flash.useFrash = true;

        yield return new WaitUntil( () => ViveController.Instance.ViveRightDown  );
        Debug.Log("トリガーを引いた");

        flashText.text.text = 1 + "回目";
        flashText.flash.useFrash = false;

    }

    IEnumerator PlayGame()
    {
        Debug.Log("SceneController.PlayGame Sta");

        int shotTimes = 0;
        timesText.text = "Times : " + (shotTimes + 1) +  "/6";
        

        //打った後のアクションをセット
        archeryController.ShotedCall = () =>
        {
            shotTimes++;
            Debug.Log("shooted : " + shotTimes + "回目");
            flashText.text.text = shotTimes + 1 + "回目";
            timesText.text = "Times : " + (shotTimes + 1) + "/6";
            archeryController.GetArrow().HitCall = () => Debug.Log("Arrow.HitCall");
        };

        

        while (shotTimes < shotTimesLimit)
        {
            yield return null;

            archeryController.UseBow();

        }

        flashText.text.text = "";
        timesText.text = "Times : " + (shotTimes) + "/6";
        yield return new WaitForSeconds(2.0f);

        Debug.Log("SceneController.PlayGame End");
    }

    IEnumerator ShotResult()
    {
        Debug.Log("リザルト");

        //1ゲーム終了、歓声
        flashText.text.text = "ゲーム終了: " + scoreTortal.TotalScore;
        flashText.flash.useFrash = true;
        yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);
        
        yield return new WaitForSeconds(1.0f);

    }

    void Reset()
    {
        //矢の破棄
        archeryController.ClearArrows();

        //UIの初期化
        scoreTortal.ResetScore();

        //点数
        
    }

    FlashTextObject CreateGameTelop()
    {
        var text_ = TextManager.Instance.addTextFrash(new Vector3(0,5,5), Vector3.one, "GameTelop", "ゲームスタート" );
        var flash_ = text_.gameObject.GetComponent<TextFrash>();
        var obj = new FlashTextObject(text_, flash_);
        return obj;
    }

}
