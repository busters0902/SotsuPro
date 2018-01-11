using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//コルーチンでゲーム進行を管理
public class ArcheryPracticeSceneController : MonoBehaviour
{

    public bool useTitle;

    public bool useTutorial;

    public bool IsGameEnd;

    [SerializeField]
    VRArcheryController3 archeryController;

    [SerializeField]
    int shotTimesLimit;

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

    [SerializeField]
    ResultController result;

    [SerializeField]
    RankingController ranking;

    [SerializeField]
    TitleController title;

    [SerializeField]
    TutorialController tutorial;

    [SerializeField]
    GameObject eyeCamera;

    bool isFullDrawing = false;

    [SerializeField]
    TutorialMovie tutorialMovie;

    [SerializeField]
    GameObject gameEndPanel;


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
        flashText.text.text = "";

        title.HideTitle();

        isFullDrawing = false;
        gameEndPanel.SetActive(false);

        ranking.panel.gameObject.SetActive(false);

        ScoreManager.Instance.scores = new System.Collections.Generic.List<Score>();

        //フェードアウト
        FadeControl.Instance.FadeIn(3, 1);

        Debug.Log("SceneController.SetupEnd");
        if (useTitle) yield return StartCoroutine(ShowTitle());
        else yield return StartCoroutine(GameMain());
    }

    //ゲームの一連の動作
    IEnumerator GameMain()
    {

        Debug.Log("SceneController.GameMain");

        yield return null;

        while(true)
        {
            if (useTutorial)
            {
                yield return StartCoroutine(PlayTutorial());
            }

            yield return StartCoroutine(StartGame());

            yield return StartCoroutine(PlayGame());

            yield return StartCoroutine(ShotResult());

            if (IsGameEnd) break;

            Reset_();
        }

        Debug.Log("SceneController.GameMain End");

    }

    IEnumerator ShowTitle()
    {
        Debug.Log("ShowTitle Start");
        //初期化
        //AudioManager.Instance.PlayBGM("bgm_title");
        title.ShowTitle();

        yield return new WaitUntil(() =>  ViveController.Instance.ViveRightDown || ViveController.Instance.ViveLeftDown || Input.GetKeyDown(KeyCode.M));

        AudioManager.Instance.PlaySE("se_decision");
        //AudioManager.Instance.StopBGM("bgm_title");
        title.HideTitle();

        Debug.Log("ShowTitle End");
        yield return StartCoroutine(GameMain());
    }


    IEnumerator StartGame()
    {
        Debug.Log("SceneController.StartGame");

        yield return null;

        Debug.Log("ゲームを開始します。トリガーを引いてください");

        flashText.text.text = "ゲームを開始します。\n\nトリガーを引いてください";
        flashText.flash.useFrash = true;
        flashText.flash.setSize(Vector3.one * 3.0f);
        flashText.flash.setPos(new Vector3( 0f, 100f, 0.5f));

        yield return new WaitUntil( () => ViveController.Instance.ViveRightDown || ViveController.Instance.ViveLeftDown);
        Debug.Log("トリガーを引いた");

        flashText.text.text = 1 + "射目";
        flashText.flash.useFrash = false;
        flashText.flash.setAlpha(1.0f);
        flashText.flash.setSize(Vector3.one * 4.0f);
        flashText.flash.transform.rotation = Quaternion.AngleAxis(15f, Vector3.up);
        flashText.flash.setPos(new Vector3( -100f, 150f, 0.5f));

    }

    IEnumerator PlayTutorial()
    {
        yield return null;

        //カメラの向き
        //右を向く (スクリーン)
        tutorial.ShowArrowAnime();

        yield return new WaitUntil(() => eyeCamera.transform.rotation.eulerAngles.y > 80);
        tutorial.HideArrowAnime();
        
        //説明をする


        yield return null;

        //左を向く (的へ)
        tutorial.Invert();
        tutorial.ShowArrowAnime();

        yield return new WaitUntil(() => eyeCamera.transform.rotation.eulerAngles.y < 10);
        tutorial.HideArrowAnime();


    }

    IEnumerator PlayGame()
    {
        Debug.Log("SceneController.PlayGame Start");

        int shotTimes = 1;
        timesText.text = "Times : " + (shotTimes) +  "/6";

        //BGM(環境音)
        //AudioManager.Instance.PlayBGM("がやがや");

        //矢をセットしたときのコールを設定
        archeryController.setArrowCall = () =>
        {
            Debug.Log("SetArrowCall");
            //環境音[ガヤガヤ]を止める
            //AudioManager.Instance.StopBGM;
        };

        //弓の弦を弾ききった時のコールを設定
        archeryController.fullDrawingCall = () =>
        {
            Debug.Log("FullDrawingCall");
            isFullDrawing = true;
        };

        //打った後のコールを設定
        archeryController.ShotedCall = () =>
        {
            Debug.Log("ShotedCall");
            if (isFullDrawing)
            {
                isFullDrawing = false;
                shotTimes++;
                Debug.Log("shotTimes : " + shotTimes + "回目");
                flashText.text.text = shotTimes + "射目";
                timesText.text = "Times : " + (shotTimes) + "/6";
                archeryController.GetArrow().HitCall = (s) =>
                {
                    Debug.Log("Arrow.HitCall");
                    //StartCoroutine(　Utility.TimerCrou(3.0f, () => AudioManager.Instance.PlayBGM("がやがや")) );
                };
                if (shotTimes >= shotTimesLimit) Debug.Log("last shoted call");
            }
            else
            {
                Debug.Log("やり直し");
            }
        };

        while (shotTimes < shotTimesLimit + 1 )
        {
            yield return null;

            archeryController.UseBow();

        }

        flashText.text.text = "";
        timesText.text = "Times : " + (shotTimes - 1 ) + "/6";
        yield return new WaitForSeconds(2.0f);

        Debug.Log("SceneController.PlayGame End");
    }

    IEnumerator ShotResult()
    {
        Debug.Log("リザルト");

        bool rankIn = false;

        //1ゲーム終了、歓声
        //flashText.text.text = "ゲーム終了: " + scoreTortal.TotalScore;
        //flashText.flash.useFrash = true;
        gameEndPanel.SetActive(true);

        result.LoadScores();

        //ランキングの更新
        var rData = DataManager.Instance.data.ranking;
        if ( rData.Any(a => a.sumPoint < ScoreManager.Instance.GetTotalScore()))
        {
            //rankingの入れ替え
            var obj = ScoreRankingData.Create();
            obj.sumPoint = ScoreManager.Instance.GetTotalScore();
            rData[rData.Length - 1] = obj;

            //rankingのソート
            rData.OrderBy(o => o.sumPoint);

            //rankingのセーブ
            DataManager.Instance.SaveData();

            rankIn = true;
        }


        //ランキングデータの読み込み
        ranking.LoadRankingData();

        yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);

        gameEndPanel.SetActive(false);

        result.panel.gameObject.SetActive(true);
        result.ShowResult();

        yield return new WaitForSeconds(5.0f);

        result.panel.gameObject.SetActive(false);
        result.HideAll();

        //ランクインしてるとき
        if (rankIn)
        {
            ranking.panel.gameObject.SetActive(true);
            ranking.ShowRanking();
            yield return new WaitForSeconds(3.0f);
            ranking.HideRanking();
            ranking.panel.gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(1.0f);
        

    }

    void Reset_()
    {
        //矢の破棄
        archeryController.ClearArrows();

        //UIの初期化
        scoreTortal.ResetScore();

        //点数
        //DataManager.Instance.roundScore = RoundScore.Create(6);
        ScoreManager.Instance.scores = new System.Collections.Generic.List<Score>();

    }

    //画面中央のテロップ
    FlashTextObject CreateGameTelop()
    {
        var text_ = TextManager.Instance.addTextFrash(new Vector3(0,5,5), Vector3.one, "GameTelop", "ゲームスタート" );
        var flash_ = text_.gameObject.GetComponent<TextFrash>();
        var obj = new FlashTextObject(text_, flash_);
        return obj;
    }

}
