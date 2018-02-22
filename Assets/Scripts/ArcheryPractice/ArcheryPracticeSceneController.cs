using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//コルーチンでゲーム進行を管理
public class ArcheryPracticeSceneController : MonoBehaviour
{

    public bool useSelectHand;

    public bool useTitle;

    public bool useTutorial;

    public bool useResult;

    public bool IsGameEnd;

    public bool isSkipGreet;

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
    SelectHandController select;

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
    GameObject playStartTarget;

    bool isFullDrawing = false;
    bool isNextTimes = false;

    [SerializeField]
    TutorialMovie tutorialMovie;

    [SerializeField]
    GameObject gameEndPanel;

    [SerializeField]
    GameObject tkfpPanel;

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
    Image timesTelopBack;

    [SerializeField]
    ArrayTexture timesTelop;

    [SerializeField]
    RectTransform telopStart;

    [SerializeField]
    RectTransform telopEnd;

    [SerializeField]
    GameObject[] startFalseObjects;

    [SerializeField]
    LiserController liser;

    [SerializeField]
    HandSelectAnimation[] handSelectAnim;
    void Start()
    {
        StartCoroutine(Setup());
    }

    //ゲームルーチン全体の立ち上げ
    IEnumerator Setup()
    {

        //FadeControl.Instance.SetGemeobject(head);

        Debug.Log("SceneController.Setup Start");

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

        foreach (var i in items)
        {
            i.obj.gameObject.SetActive(false);
        }

        UI3DManager.Instance.loadUIAnimation(items);

        title.HideTitle();

        isFullDrawing = false;
        gameEndPanel.SetActive(false);
        tkfpPanel.SetActive(false);

        ranking.panel.gameObject.SetActive(false);

        ScoreManager.Instance.scores = new System.Collections.Generic.List<Score>();

        tutorialMovie.pauseMovie();

        timesTelop.gameObject.SetActive(false);
        timesTelopBack.gameObject.SetActive(false);

        select.Hide();

        //フェードアウト
        //FadeControl.Instance.FadeIn(3, 1);

        result.Initialize();

        ranking.CreateRankingPanel();

        Debug.Log("SceneController.Setup End");
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

            if (useSelectHand)
            {
                yield return StartCoroutine(SelectHand());
            }

            if (useTutorial)
            {
                if (isSkipGreet == false) yield return StartCoroutine(Greet());
                yield return StartCoroutine(ShowTutorial());
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

    //利き腕選択
    IEnumerator SelectHand()
    {
        Debug.Log("SceneController.SelectHand Start");

        //liserの初期化
        liser.onUsed = true;
        liser.onUseShow = false;
        liser.onUseLiser = true;
        liser.line.enabled = true;

        liser.targetNames = select.targetNames;
        handSelectAnim[2].gameObject.SetActive(true);
        StartCoroutine(Utility.TimerCrou(0.1f, () =>
        {
            handSelectAnim[2].animationStart();
        }));
        select.Show();

        bool isRight = false;
        bool isLeft = false;

        liser.SetAction((n) =>
        {
            if (n == 0) isRight = true;
            else isLeft = true;
        });

        //選んだオブジェクトで判定
        yield return new WaitUntil(
            () =>
            {
                return (isRight || isLeft);
            });

        if (isLeft) select.SetLeftMode();
        if (isRight) select.SetRightMode();
        AudioManager.Instance.PlaySE("se_decision");
        handSelectAnim[2].gameObject.SetActive(false);
        select.Hide();

        liser.onUsed = true;
        liser.onUseShow = true;
        liser.onUseLiser = true;
        liser.line.enabled = false;

        yield return new WaitForSeconds(1.0f);

        Debug.Log("SceneController.SelectHand End");
    }

    //タイトル画面
    IEnumerator ShowTitle()
    {

        Debug.Log("ShowTitle Start");

        flashText.text.text = "";

        //初期化
        AudioManager.Instance.PlayBGM("bgm_title");
        AudioManager.Instance.setBGMLoop(true);

        for (int i = 0; i < 2; i++)
        {
            handSelectAnim[i].gameObject.SetActive(true);
        }
        for (int i = 0; i < 2; i++)
        {
            handSelectAnim[i].animationStart();
        }

        title.ShowTitle();

        yield return new WaitUntil(() => ViveController.Instance.ViveRightDown || ViveController.Instance.ViveLeftDown || Input.GetKeyDown(KeyCode.M));
        for (int i = 0; i < 2; i++)
        {
            handSelectAnim[i].gameObject.SetActive(false);
        }
        AudioManager.Instance.PlaySE("se_decision");
        AudioManager.Instance.StopBGM("bgm_title");
        title.HideTitle();

        Debug.Log("ShowTitle End");

    }

    //ゲーム開始
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

        AnimateTelop();

        Debug.Log("End StartGame");
    }

    IEnumerator Greet()
    {
        yield return new WaitForSeconds(1.5f);


        ////チュートリアル中断
        //StartCoroutine(Stopper(() => StopCoroutine(  ),
        //    () =>
        //    {
        //        //Debug.Log("cehck");
        //        if (ViveController.Instance.ViveLeftUp) Debug.Log("crou end");

        //        return ViveController.Instance.ViveLeftUp;
        //    }
        //));


        bool endf = false;
        AudioManager.Instance.PlaySE("01", () => endf = true);

        yield return new WaitUntil(() => endf);

        endf = false;

        yield return new WaitForSeconds(0.5f);

        AudioManager.Instance.PlaySE("02", () => endf = true);

        yield return new WaitUntil(() => endf);

    }

    //練習
    IEnumerator Practice()
    {

        //コールバック仮
        archeryController.bow.arrowSetHitCall = (s, p) =>
        {
            Debug.Log("ArrowHitCall " + s + ": " + p);
            //的に当たった場合
            if (s.name == "Mato")
            {
                Debug.Log("！！　やったぜ");
                var mato = s.GetComponent<Mato>();

                //スコア
                int score = mato.calc.getScore(p);
                Debug.Log("スコア :" + score);
                mato.hitStop.EffectPlay(p, score);
                //ScoreManager.Instance.AddScore(shotTimes, score);

                //SE
                AudioManager.Instance.PlaySE("的に当たる");

                mato.hitStop.OnHitUpdateText(score);

            }
            else if (s.name == "BackWallQuad")
            {
                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
                mato.hitStop.OnHitUpdateText(0);
            }
            else
            {
                Debug.Log("！！　はずれ");
                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
                mato.hitStop.OnHitUpdateText(0);
            }

        };

        archeryController.setArrowCall = () =>
        {
            Debug.Log("setArrowCall");

            archeryController.bow.arrow.targets.Add(mato.quadCollider);
            archeryController.bow.arrow.frontWall = frontWall;

            archeryController.bow.arrow.useCalcIntersectWall = true;

            archeryController.bow.SetShotCall();
        };

        archeryController.fullDrawingCall = () =>
        {
            Debug.Log("fullDrawingCall");
        };

        archeryController.ShotedCall = () =>
        {
            Debug.Log("ShotedCall");
        };

        while (true)
        {
            yield return null;

            archeryController.UseBow();
        }

    }

    //チュートリルを見せる
    IEnumerator ShowTutorial()
    {

        yield return null;
        Debug.Log("SceneController.PlayTutorial Start");

        Coroutine subCrou = StartCoroutine(Practice());

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

        //bool end_flag = false;      

        //チュートリアル中断
        //StartCoroutine(Stopper(() => tutorialAnimationController.animationStop(),
        //    () =>
        //    {
        //        //Debug.Log("cehck");
        //        return ViveController.Instance.ViveLeftUp;
        //    }
        //));

        //チュートリアルモーションムービー
        yield return StartCoroutine(tutorialAnimationController.waitAnimation(false));

        string dir2 = "Dir2";
        UI3DManager.Instance.showUI(dir2);

        string eye2 = "Eye2";
        UI3DManager.Instance.showUI(eye2);

        //正面を見るまで待つ
        yield return new WaitUntil(() =>
        {
            var tgtDir = playStartTarget.transform.position;
            var cross = Vector3.Cross(eyeCamera.transform.forward, tgtDir);
            if (cross.y >= 0) return true;
            return false;
        });

        UI3DManager.Instance.hideUI(dir2);
        UI3DManager.Instance.hideUI(eye2);

        StopCoroutine(subCrou);
        //※弓や矢のリセット
        //archeryController.ClearArrows();
        archeryController.Reset();

    }

    IEnumerator PlayGame()
    {
        Debug.Log("SceneController.PlayGame Start");

        int shotTimes = 1;
        timesText.text = (shotTimes) + "/6";
        timesTelop.ChangeTexture(shotTimes - 1);
        timesTelop.gameObject.SetActive(true);
        timesTelopBack.gameObject.SetActive(true);

        //BGM(環境音)
        AudioManager.Instance.PlaySE("gaya", gayaTransfom.localPosition).loop = true;
        Debug.Log("PlayBGM がやがや");

        //チュートリアルのボタンの設定
        liser.Setup(tutorialAnimationController.targetNames, tutorialAnimationController.ChangeChannel);

        ///////////////////////////////////////////////////////////////////////
        //ゲーム中のコールバックのセッティング

        bool _liser = false ;
        bool lockBow = false ;

        //矢が衝突したときの細かい処理　点数計算、エフェクト、有効、SE、スコアの追加
        archeryController.bow.arrowSetHitCall = (s, p) =>
        {
            Debug.Log("ArrowHitCall " + s + ": " + p);

            Debug.Log("shotTimes : " + shotTimes + "回目");

            //archeryController.canReload = true;
            StartCoroutine(Utility.TimerCrou(2.0f, () =>
            {
                archeryController.canReload = true;
                timesText.text = (shotTimes) + "/6";
                timesTelop.ChangeTexture(shotTimes - 1);
                AnimateTelop();
            }));

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

                Debug.Log("！！　はずれ");
                AudioManager.Instance.PlaySE("kansei_2", gayaTransfom.localPosition);

                ScoreManager.Instance.AddScore(shotTimes, 0);
                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
            }

            //if(shotTimes < 6)
            shotTimes++;

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

            archeryController.canReload = false;
            lockBow = true;

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

            //レーザーが表示されてるときにリロード出来ない
            archeryController.canReload2 = !liser.GetLineEnable();

            archeryController.UseBow();
        }

        timesTelop.gameObject.SetActive(false);
        timesTelopBack.gameObject.SetActive(false);
        flashText.text.text = "";
        timesText.text = "6/6";

        yield return new WaitForSeconds(2.0f);

        //7/6
        timesText.text = "6/6";

        Debug.Log("SceneController.PlayGame End");
    }

    IEnumerator ShowResult()
    {
        Debug.Log("リザルト");

        AudioManager.Instance.PlayBGM("bgm_result");

        bool rankIn = false;

        int score = ScoreManager.Instance.GetTotalScore();

        //1ゲーム終了、歓声
        gameEndPanel.SetActive(true);

        result.Load();

        int rank = -1;

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

            //配列の長さがランキングの順位になるので
            //rank = DataManager.Instance.data.ranking.Where((s) => s.sumPoint >= score).ToArray().Length;
            rank = DataManager.Instance.data.ranking.Count((s) => s.sumPoint >= score);
        }
        else
        {
            rank = 100 - 1;
        }

        Debug.Log(string.Format("現在のランキングは{0}です", rank));

        //ランキングデータの読み込み
        //yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);

        ranking.LoadRankingData();

        //yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);

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

        //yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);

        yield return new WaitForSeconds(3.0f);

        ranking.posReset();
        //反転
        var trans = result.panel.transform;

        yield return StartCoroutine(Utility.TimeCrou(0.5f, (f) => trans.rotation = Quaternion.EulerAngles(0, Mathf.PI * 0.5f * f, 0)));

        result.SetActiveResultPanel(false);
        result.HideAll();

        ranking.panel.gameObject.SetActive(true);
        ranking.ShowRanking();

        //ランクインしたら色を黄色に
        if (rank > 0 && rank < ranking.panel.rankScores.Length)
        {
            ranking.panel.rankScores[rank - 1].plateImage.color = Color.yellow;
        }

        var trans2 = ranking.panel.transform;

        yield return StartCoroutine(Utility.TimeCrou(0.5f, (f) => trans2.rotation = Quaternion.EulerAngles(0, Mathf.PI * (0.5f - 1.0f + (0.5f * f)), 0)));

        Debug.Log("ランキングをイージングします");

        var rankingmoveFlag = false;
        ranking.moveContent(rank, () => rankingmoveFlag = true);
        yield return new WaitUntil(() => rankingmoveFlag);

        yield return new WaitForSeconds(3.0f);
        //yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);

        ranking.HideRanking();
        ranking.panel.gameObject.SetActive(false);

        //変更した色をもどす
        if (rank > 0 && rank < ranking.panel.rankScores.Length)
        {
            ranking.panel.rankScores[rank - 1].plateImage.color = Color.white;
        }

        yield return new WaitForSeconds(1.0f);

        tkfpPanel.gameObject.SetActive(true);

        //yield return new WaitUntil(() => ViveController.Instance.ViveRightDown);
        yield return new WaitForSeconds(10.0f);

        tkfpPanel.gameObject.SetActive(false);

        AudioManager.Instance.StopBGM("bgm_result");

        yield return new WaitForSeconds(1.0f);

    }

    void Reset_()
    {

        //矢の破棄
        archeryController.ClearArrows();

        //UIの初期化
        scoreTortal.ResetScore();
        timesText.text = "0/6";
        mato.hitStop.OnHitUpdateText(0);

        tutorial.Reset_();
        tutorialMovie.stopMovie();
        result.panel.transform.rotation = Quaternion.Euler(0, 0, 0);

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

    IEnumerator Stopper(System.Action act, System.Func<bool> boolFunc)
    {
        stEnd = false;

        while (stEnd == false)
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

    //LiserControllerの初期化
    //※
    void InitializeLiser()
    {
        //liser.SetAction();
        //liser.targetNames();

        liser.onUsed = false;
        liser.onUseShow = true;
        liser.onUseLiser = true;
        liser.line.enabled = false;
    }

    void AnimateTelop()
    {
        timesTelop.gameObject.transform.position = telopStart.position;

        //iTween.MoveTo( timesTelop.gameObject, telopEnd.position, 1.0f );
        iTween.MoveTo(timesTelop.gameObject, iTween.Hash(
            "x", telopEnd.position.x,
            "time", 1,
            "easetype", "easeOutBack"));

    }

}