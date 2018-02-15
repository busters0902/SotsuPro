using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//コルーチンでゲーム進行を管理
public class ArcheryPracticeSceneController : MonoBehaviour
{

    public bool useTitle;

    public bool useTutorial;

    public bool useResult;

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

    [SerializeField]
    Text scoreText;

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
    GarraryController garrary;

    [SerializeField]
    RightHandAnim rightHand;

    [SerializeField]
    GameObject eyeCamera;

    [SerializeField]
    GameObject tutorialTarget;

    [SerializeField]
    GameObject PlayStartTarget;

    bool isFullDrawing = false;
    bool isNextTimes = false;

    [SerializeField]
    TutorialMovie tutorialMovie;

    [SerializeField]
    GameObject gameEndPanel;

    [SerializeField]
    Mato mato;

    [SerializeField]
    QuadCollider frontWall;

    [SerializeField]
    TutorialAnimationController tutorialAnimationController;

    [SerializeField]
    Transform gayaTransfom;

    [SerializeField]
    UIAnimationItem[] items;

    [SerializeField]
    ArrayTexture timesTelop;

    [SerializeField]
    GameObject[] startFalseObjects;

    void Start()
    {
        StartCoroutine(Setup());
    }

    //ゲームルーチン全体の立ち上げ
    IEnumerator Setup()
    {

        //FadeControl.Instance.SetGemeobject(head);

        Debug.Log("SceneController.Setup");

        yield return null;

        foreach (var i in startFalseObjects)
        {
            i.gameObject.SetActive(false);
        }

        TextManager.Instance.SetCanvas(canvas);
        TextManager.Instance.SetPrefab((GameObject)Resources.Load("Model/Prefabs/Text"));

        flashText = CreateGameTelop();
        flashText.Set(flashTextSettings);
        flashText.text.text = "";

        foreach( var i in items )
        {
            i.obj.gameObject.SetActive(false);
        }

        UI3DManager.Instance.loadUIAnimation(items);

        title.HideTitle();

        isFullDrawing = false;
        gameEndPanel.SetActive(false);

        ranking.panel.gameObject.SetActive(false);

        ScoreManager.Instance.scores = new System.Collections.Generic.List<Score>();

        tutorialMovie.pauseMovie();

        timesTelop.gameObject.SetActive(false);

        //フェードアウト
        //FadeControl.Instance.FadeIn(3, 1);

        result.Initialize();

        Debug.Log("SceneController.SetupEnd");
        yield return StartCoroutine(GameMain());
    }

    //ゲームの一連の動作
    IEnumerator GameMain()
    {

        Debug.Log("SceneController.GameMain");
        yield return null;

        while (true)
        {

            if (useTitle)
            {
                yield return StartCoroutine(ShowTitle());
            }

            if (useTutorial)
            {
                yield return StartCoroutine(PlayTutorial());
            }

            yield return StartCoroutine(StartGame());

            yield return StartCoroutine(PlayGame());

            if (useResult)
            {
                yield return StartCoroutine(ShowResult());
            }


            if (IsGameEnd) break;

            Reset_();
        }

        Debug.Log("SceneController.GameMain End");

    }

    IEnumerator ShowTitle()
    {
        Debug.Log("ShowTitle Start");

        flashText.text.text = "";

        //初期化
        AudioManager.Instance.PlayBGM("bgm_title");
        AudioManager.Instance.setBGMLoop(true);

        title.ShowTitle();

        yield return new WaitUntil(() => ViveController.Instance.ViveRightDown || ViveController.Instance.ViveLeftDown || Input.GetKeyDown(KeyCode.M));

        AudioManager.Instance.PlaySE("se_decision");
        AudioManager.Instance.StopBGM("bgm_title");
        title.HideTitle();

        Debug.Log("ShowTitle End");

    }


    IEnumerator StartGame()
    {
        Debug.Log("Start StartGame");

        yield return null;

        Debug.Log("ゲームを開始します。トリガーを引いてください");

        flashText.text.text = "ゲームを開始します。\n\nトリガーを引いてください";
        flashText.flash.useFrash = true;
        flashText.flash.setColor(0, 1, 1);
        flashText.flash.setSize(Vector3.one * 3.0f);
        flashText.flash.transform.rotation = Quaternion.AngleAxis(0f, Vector3.right);
        flashText.flash.setPos(new Vector3(0f, 100f, 0.5f));
        flashText.text.fontSize = 20;

        //スタート表示、トリガー待ち
        yield return new WaitUntil(() => ViveController.Instance.ViveRightDown || ViveController.Instance.ViveLeftDown);
        AudioManager.Instance.PlaySE("se_decision");

        flashText.text.text = " ";
        //flashText.text.text = 1 + "射目";
        flashText.flash.useFrash = false;
        flashText.flash.setColor(1, 1, 1);
        flashText.flash.setAlpha(1.0f);
        flashText.flash.setSize(Vector3.one * 1.0f);
        flashText.flash.transform.rotation = Quaternion.AngleAxis(30f, Vector3.right);
        flashText.flash.setPos(new Vector3(2.0f, -125.1f, 86.0f));
        flashText.text.fontSize = 90;
        mato.hitStop.OnHitUpdateText(0);

        Debug.Log("End StartGame");
    }

    IEnumerator PlayTutorial()
    {
        yield return null;
        Debug.Log("SceneController.PlayTutorial Start");

        //カメラの向き
        //右を向く (スクリーン)
        tutorial.ShowArrowAnime();
        string dir1 = "Dir1";
        UI3DManager.Instance.showUI(dir1);

        string eye1 = "Eye1";
        UI3DManager.Instance.showUI(eye1);

        //チュートリアル版を見るまで待つ
        yield return new WaitUntil(() =>
        {
            var tgtDir = tutorialTarget.transform.position - eyeCamera.transform.position;
            var cross = Vector3.Cross(eyeCamera.transform.forward, tgtDir);
            if (cross.y <= 0) return true;
            return false;
        });

        tutorial.HideArrowAnime();

        UI3DManager.Instance.hideUI(dir1);
        UI3DManager.Instance.hideUI(eye1);

        //説明をする
        //tutorialAnimationController.SetFlow(true);

        ////tutorialMovie.playMovie();
        ////tutorialMovie.SetLoop(false);

        bool end_flag = false;
        //StartCoroutine(Utility.TimerCrou(5, () =>
        //{
        //    end_flag = true;
        //    tutorialAnimationController.SetFlow(false);
        //}));

        StartCoroutine(TutorialStopper(() => tutorialAnimationController.Stop(),
            () =>
            {
                 Debug.Log("cehck");
                return ViveController.Instance.ViveLeftUp;
            }
        ));

        yield return StartCoroutine(tutorialAnimationController.waitAnimation());

        //動画終了
        //yield return new WaitUntil(() =>
        //{
        //    if (ViveController.Instance.ViveLeftDown) return true;
        //    if (Input.GetKeyDown(KeyCode.N)) return true;
        //    return end_flag;
        //    //return tutorialMovie.IsEndMovie();
        //});

        //tutorialMovie.stopMovie();

        //左を向く (的へ)
        //tutorial.Invert();
        //tutorial.ShowArrowAnime();

        string dir2 = "Dir2";
        UI3DManager.Instance.showUI(dir2);

        string eye2 = "Eye2";
        UI3DManager.Instance.showUI(eye2);

        //正面を見るまで待つ
        yield return new WaitUntil(() =>
        {
            //var tgtDir = Vector3.forward;
            var tgtDir = PlayStartTarget.transform.position;
            var cross = Vector3.Cross(eyeCamera.transform.forward, tgtDir);
            if (cross.y >= 0) return true;
            return false;
        });

        UI3DManager.Instance.hideUI(dir2);
        UI3DManager.Instance.hideUI(eye2);
        //tutorial.HideArrowAnime();

    }

    IEnumerator PlayGame()
    {
        Debug.Log("SceneController.PlayGame Start");

        int shotTimes = 1;
        //timesText.text = "Times : " + (shotTimes) + "/6";
        timesText.text = (shotTimes) + "/6";
        timesTelop.ChangeTexture(shotTimes - 1);
        timesTelop.gameObject.SetActive(true);

        //BGM(環境音)
        //※
        AudioManager.Instance.PlaySE("gaya", gayaTransfom.localPosition).loop = true;
        Debug.Log("PlayBGM がやがや");

        ///////////////////////////////////////////////////////////////////////
        //ゲーム中のコールバックのセッティング

        //矢が衝突したときの細かい処理　点数計算、エフェクト、有効、SE、スコアの追加
        archeryController.bow.arrowSetHitCall = (s, p) =>
        {
            Debug.Log("ArrowHitCall " + s + ": " + p);

            Debug.Log("shotTimes : " + shotTimes + "回目");

            //archeryController.canReload = true;

            //的に当たった場合
            if (s.name == "Mato")
            {
                Debug.Log("！！　やったぜ");
                var mato = s.GetComponent<Mato>();

                //スコア
                int score = mato.calc.getScore(p);
                Debug.Log("スコア :" + score);
                mato.hitStop.EffectPlay(p, score);
                ScoreManager.Instance.AddScore(shotTimes, score);

                //SE
                AudioManager.Instance.PlaySE("的に当たる");
                StartCoroutine(Utility.TimerCrou(1f,
                    () =>
                    {
                        garrary.highJump();
                        //AudioManager.Instance.PlaySE("kansei_1");
                        if (score <= 3)
                        {
                            AudioManager.Instance.PlaySE("kansei_3", gayaTransfom.localPosition);

                            //ディレイをかける場合
                            //StartCoroutine(Utility.TimerCrou(0.5f, () => AudioManager.Instance.PlaySE("kansei_1")));
                        }
                        else if (score <= 5)
                        {
                            AudioManager.Instance.PlaySE("kansei_1", gayaTransfom.localPosition);
                            AudioManager.Instance.PlaySE("hakushu_2", gayaTransfom.localPosition);
                        }
                        else if (score <= 8)
                        {
                            AudioManager.Instance.PlaySE("kansei_4", gayaTransfom.localPosition);
                            AudioManager.Instance.PlaySE("hakushu_2", gayaTransfom.localPosition);

                        }
                        else //9,10点
                        {
                            AudioManager.Instance.PlaySE("kansei_5", gayaTransfom.localPosition);
                            AudioManager.Instance.PlaySE("hakushu_1", gayaTransfom.localPosition);
                        }
                        Debug.Log("SE kansei_1 ");
                        StartCoroutine(Utility.TimerCrou(4.0f, () => garrary.stopHighJump()));
                    }
                ));

                //UIの更新
                scoreTortal.AddScore(score);
                mato.hitStop.OnHitUpdateText(score);

            }
            else if (s.name == "BackWallQuad")
            {

                Debug.Log("！！　惜しい");
                AudioManager.Instance.PlaySE("kansei_6", gayaTransfom.localPosition);

                ScoreManager.Instance.AddScore(shotTimes, 0);
                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
            }
            else
            {
                //※
                Debug.Log("！！　はずれ");
                AudioManager.Instance.PlaySE("kansei_2", gayaTransfom.localPosition);
                
                ScoreManager.Instance.AddScore(shotTimes, 0);
                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
            }

            shotTimes++;
            //flashText.text.text = shotTimes + "射目";
            //flashText.text.text = shotTimes.ToString();
            //timesText.text = "Times : " + (shotTimes) + "/6";
            timesText.text = (shotTimes) + "/6";
            timesTelop.ChangeTexture(shotTimes-1);
            isNextTimes = false;

        };

        //矢をセットしたときのコールを設定
        archeryController.setArrowCall = () =>
        {
            Debug.Log("SetArrowCall");

            //環境音[ガヤガヤ]を止める
            AudioManager.Instance.FadeOutSE("gaya", 0.5f);

            archeryController.bow.arrow.targets.Add(mato.quadCollider);
            archeryController.bow.arrow.frontWall = frontWall;

            archeryController.bow.arrow.useCalcIntersectWall = true;

            //archeryController.canReload = false;

            archeryController.bow.SetShotCall();

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
            Debug.Log("Shoted Call");
            if (isFullDrawing)
            {
                isFullDrawing = false;
                isNextTimes = true;

                Debug.Log("Set ArrowHitCall");
                if (shotTimes > shotTimesLimit) Debug.Log("Last shoted call");
            }
            else
            {
                Debug.Log("やり直し");
            }
        };

        ///////////////////////////////////////////////////////////////////////
        //射撃ができるループ
        while (shotTimes < shotTimesLimit + 1)
        {
            yield return null;

            archeryController.UseBow();

        }

        timesTelop.gameObject.SetActive(false);
        flashText.text.text = "";
        //timesText.text = "Times : " + 6 + "/6";
        timesText.text = 6 + "/6";

        yield return new WaitForSeconds(2.0f);

        Debug.Log("SceneController.PlayGame End");
    }

    IEnumerator ShowResult()
    {
        Debug.Log("リザルト");

        bool rankIn = false;

        int score = ScoreManager.Instance.GetTotalScore();

        //1ゲーム終了、歓声
        gameEndPanel.SetActive(true);

        result.Load();

        //ランキングの更新
        if (DataManager.Instance.IsRankIn(score))
        {
            var rankData = ScoreRankingData.Create(System.DateTime.Now.Minute, System.DateTime.Now.ToString(), score);
            DataManager.Instance.AddRanking(rankData);

            rankIn = true;
        }

        if (rankIn == true)
        {

            DataManager.Instance.data.ranking = DataManager.Instance.data.ranking.OrderByDescending((s) => s.sumPoint).ToArray();
            DataManager.Instance.SaveData();

        }

        //ランキングデータの読み込み
        ranking.LoadRankingData();

        //トリガーを
        flashText.text.text = "トリガーを引いてください";
        flashText.flash.useFrash = true;
        flashText.flash.setColor(0, 1, 1);
        flashText.flash.setSize(Vector3.one * 2.0f);
        flashText.flash.transform.rotation = Quaternion.AngleAxis(0f, Vector3.right);
        flashText.flash.setPos(new Vector3(0f, -60f, 0.5f));
        flashText.text.fontSize = 20;


        //リザルト表示する
        yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);
        AudioManager.Instance.PlaySE("se_decision");

        flashText.text.text = "";


        gameEndPanel.SetActive(false);

        result.SetActiveResultPanel(true);
        result.ShowResult(() => AudioManager.Instance.PlaySE("いえーい"));


        yield return new WaitForSeconds(3.0f);

        //反転
        var trans = result.panel.transform;
        
        yield return StartCoroutine( Utility.TimeCrou(0.5f, (f) => trans.rotation = Quaternion.EulerAngles( 0, Mathf.PI * 0.5f * f, 0)));

        result.SetActiveResultPanel(false);
        result.HideAll();

        ranking.panel.gameObject.SetActive(true);
        ranking.ShowRanking();

        //rankingマーカー

        var trans2 = ranking.panel.transform;

        yield return StartCoroutine( Utility.TimeCrou(0.5f, (f) => trans2.rotation = Quaternion.EulerAngles( 0, Mathf.PI * (0.5f - 1.0f + (0.5f * f)) , 0)));

        yield return new WaitForSeconds(3.0f);

        ranking.HideRanking();
        ranking.panel.gameObject.SetActive(false);


        yield return new WaitForSeconds(1.0f);

    }

    void Reset_()
    {

        //矢の破棄
        archeryController.ClearArrows();

        //UIの初期化
        scoreTortal.ResetScore();
        tutorial.Reset_();
        tutorialMovie.stopMovie();
        result.panel.transform.rotation = Quaternion.EulerAngles(0, 0, 0);

        //点数
        ScoreManager.Instance.scores = new System.Collections.Generic.List<Score>();

    }

    //画面中央のテロップ
    FlashTextObject CreateGameTelop()
    {
        var text_ = TextManager.Instance.addTextFrash(new Vector3(0, 5, 5), Vector3.one, 20, "GameTelop", "ゲームスタート");
        var flash_ = text_.gameObject.GetComponent<TextFrash>();
        var obj = new FlashTextObject(text_, flash_);
        return obj;
    }

    bool stEnd = false;

    IEnumerator TutorialStopper(System.Action act, System.Func<bool> boolFunc)
    {
        while (!stEnd)
        {
            yield return null;
            
            if (boolFunc())
            {
                Debug.Log("cehck true");
                act();
                yield break;
            }
        }

        yield return null;
    }


}