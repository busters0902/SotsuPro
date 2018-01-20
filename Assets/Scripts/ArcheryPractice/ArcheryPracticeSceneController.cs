﻿using System.Collections;
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
    GameObject eyeCamera;

    [SerializeField]
    GameObject tutorialTarget;

    bool isFullDrawing = false;

    [SerializeField]
    TutorialMovie tutorialMovie;

    [SerializeField]
    GameObject gameEndPanel;

    [SerializeField]
    Mato mato;

    [SerializeField]
    QuadCollider frontWall;

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

        tutorialMovie.pauseMovie();

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
        AudioManager.Instance.PlayBGM("bgm_title");
        title.ShowTitle();

        yield return new WaitUntil(() =>  ViveController.Instance.ViveRightDown || ViveController.Instance.ViveLeftDown || Input.GetKeyDown(KeyCode.M));

        AudioManager.Instance.PlaySE("se_decision");
        AudioManager.Instance.StopBGM("bgm_title");
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
        flashText.text.fontSize = 20;

        //スタート表示、トリガー待ち
        yield return new WaitUntil( () => ViveController.Instance.ViveRightDown || ViveController.Instance.ViveLeftDown);
        Debug.Log("トリガーを引いた");

        flashText.text.text = 1 + "射目";
        flashText.flash.useFrash = false;
        flashText.flash.setAlpha(1.0f);
        flashText.flash.setSize(Vector3.one * 1.0f);
        flashText.flash.transform.rotation = Quaternion.AngleAxis(30f, Vector3.right);
        flashText.flash.setPos(new Vector3( 2.0f, -125.1f, 86.0f));
        flashText.text.fontSize = 90;

    }

    IEnumerator PlayTutorial()
    {
        yield return null;

        //カメラの向き
        //右を向く (スクリーン)
        tutorial.ShowArrowAnime();

        //チュートリアル版を見るまで待つ
        yield return new WaitUntil(() => 
        {
            var tgtDir = tutorialTarget.transform.position - eyeCamera.transform.position;
            var cross = Vector3.Cross(eyeCamera.transform.forward, tgtDir);
            Debug.Log("cross :" + cross.y);
            if (cross.y <= 0) return true;
            return false;
        });
        tutorial.HideArrowAnime();

        //説明をする
        tutorialMovie.playMovie();
        tutorialMovie.SetLoop(false);

        //動画終了
        yield return new WaitUntil(() =>
        {
            if (ViveController.Instance.ViveLeftDown) return true;
            if (Input.GetKeyDown(KeyCode.N)) return true;
            return tutorialMovie.IsEndMovie();
        });

        //左を向く (的へ)
        tutorial.Invert();
        tutorial.ShowArrowAnime();

        //正面を見るまで待つ
        yield return new WaitUntil(() =>
        {
            var tgtDir = Vector3.forward;
            var cross = Vector3.Cross(eyeCamera.transform.forward, tgtDir);
            if (cross.y >= 0) return true;
            return false;
        });

        tutorial.HideArrowAnime();

    }

    IEnumerator PlayGame()
    {
        Debug.Log("SceneController.PlayGame Start");

        int shotTimes = 1;
        timesText.text = "Times : " + (shotTimes) +  "/6";

        //BGM(環境音)
        AudioManager.Instance.PlayBGM("がやがや");
        Debug.Log("PlayBGM がやがや");

        //
        //矢が衝突したときの細かい処理　点数計算、エフェクト、有効、SE、スコアの追加
        archeryController.bow.arrowSetHitCall = (s, p) =>
        {
            Debug.Log("ArrowHitCall " + s + ": " + p);
            //StartCoroutine(　Utility.TimerCrou(3.0f, () => AudioManager.Instance.PlayBGM("がやがや")) );

            //archeryController.canReload = true;

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
                StartCoroutine(Utility.TimerCrou(0.5f,
                    () =>
                    {
                        AudioManager.Instance.PlaySE("kansei_1");
                        Debug.Log("SE kansei_1 ");
                    }
                ));

                //UIの更新
                scoreTortal.AddScore(score);
                mato.hitStop.OnHitUpdateText(score);


            }
            else if (s.name == "BackWallQuad")
            {
                Debug.Log("！！　惜しい");
                ScoreManager.Instance.AddScore(shotTimes, 0);
                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
            }
            else
            {
                Debug.Log("！！　はずれ");
                ScoreManager.Instance.AddScore(shotTimes, 0);
                //mato.hitStop.OnHitUpdateText(0);

                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
            }

        };

        //矢をセットしたときのコールを設定
        archeryController.setArrowCall = () =>
        {
            Debug.Log("SetArrowCall");
            
            //環境音[ガヤガヤ]を止める
            AudioManager.Instance.StopBGM("がやがや");

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
                shotTimes++;
                Debug.Log("shotTimes : " + shotTimes + "回目");
                flashText.text.text = shotTimes + "射目";
                timesText.text = "Times : " + (shotTimes) + "/6";

                
                //archeryController.bow.arrow.HitCall = (s,p) =>
                //{
                //    Debug.Log("ArrowHitCall "+ s+ ": "+ p);
                //    //StartCoroutine(　Utility.TimerCrou(3.0f, () => AudioManager.Instance.PlayBGM("がやがや")) );

                //    //archeryController.canReload = true;

                //    if (s.name == "Mato")
                //    {
                //        Debug.Log("！！　やったぜ");
                //        var mato = s.GetComponent<Mato>();

                //        //スコア
                //        int score = mato.calc.getScore(p);
                //        Debug.Log("スコア :" + score);
                //        mato.hitStop.EffectPlay(p, score);
                //        ScoreManager.Instance.AddScore(shotTimes, score);

                //        //SE
                //        AudioManager.Instance.PlaySE("的に当たる");
                //        StartCoroutine(Utility.TimerCrou(0.5f,
                //            () =>
                //            {
                //                AudioManager.Instance.PlaySE("kansei_1");
                //                Debug.Log("SE kansei_1 ");
                //            }
                //        ));

                //        //UIの更新
                //        scoreTortal.AddScore(score);
                //        mato.hitStop.OnHitUpdateText(score);


                //    }
                //    else if(s.name == "BackWallQuad")
                //    {
                //        Debug.Log("！！　惜しい");
                //        ScoreManager.Instance.AddScore(shotTimes, 0);
                //        AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
                //    }
                //    else
                //    {
                //        Debug.Log("！！　はずれ");
                //        ScoreManager.Instance.AddScore(shotTimes, 0);
                //        //mato.hitStop.OnHitUpdateText(0);

                //        AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
                //    }

                //};
                Debug.Log("Set ArrowHitCall");
                if (shotTimes >= shotTimesLimit) Debug.Log("Last shoted call");
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
        gameEndPanel.SetActive(true);

        result.Load();

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
        tutorial.Reset_();
        tutorialMovie.stopMovie();

        //点数
        //DataManager.Instance.roundScore = RoundScore.Create(6);
        ScoreManager.Instance.scores = new System.Collections.Generic.List<Score>();

    }

    //画面中央のテロップ
    FlashTextObject CreateGameTelop()
    {
        var text_ = TextManager.Instance.addTextFrash(new Vector3(0,5,5), Vector3.one, 20,"GameTelop", "ゲームスタート" );
        var flash_ = text_.gameObject.GetComponent<TextFrash>();
        var obj = new FlashTextObject(text_, flash_);
        return obj;
    }

}