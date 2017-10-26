using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunGameSceneController : MonoBehaviour
{

    [SerializeField]
    SteamVR_TrackedObject lController;

    [SerializeField]
    SteamVR_TrackedObject rController;  

    [SerializeField]
	EnemyController enemyController;

    [SerializeField]
    GunController gunRController;

    [SerializeField]
    GunController gunLController;

    [SerializeField]
	Text text;

	[SerializeField, Tooltip("残り時間")]
	Text remTimeText;

	[SerializeField]
	Text scoreText;

	[SerializeField]
	Text resultText;

	//制限時間を計測
	Timer timer;

	float remTime;

	[SerializeField]
	float limitTime;

	public bool isGameEnd;

	public Func<bool> inputTriggerDown = null;

	// = 倒した数
	public int Score{ get; private set; }

    bool viveRightDown;
    bool viveRightUp;
    bool viveRight;

    void Awake()
	{
		VRUtility.CreateVRController();
    }


	void Start()
	{
        ViveController.Instance.SetLeftController(lController);
        ViveController.Instance.SetRightController(rController);

        ////inputTriggerDown = () => Input.GetMouseButtonDown(0);
        //inputTriggerDown = () =>
        //{
        //    Debug.Log("いんぷっとチェック");
        //    //var dev = ViveController.Instance.GetRightDevice();
        //    //bool v = dev.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
        //    bool v = false;
        //    Debug.Log(v);
        //    return v;
        //};

        Init();

		StartCoroutine(PlayScene());

		timer = new Timer();
        //timer.time

        Debug.Log("start end");
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Score++;
		}


        var device = ViveController.Instance.GetRightDevice();
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            viveRightDown = true;
        else
            viveRightDown = false;

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            viveRightUp = true;
        else
            viveRightUp = false;
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            viveRight = true;
        else
            viveRight = false;

    }

    

    bool GetRightDown()
    {
        var device = ViveController.Instance.GetRightDevice();
        return device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger);
    }

	void Init()
	{
		text.gameObject.SetActive(true);
		remTimeText.gameObject.SetActive(true);
		scoreText.gameObject.SetActive(true);
		resultText.gameObject.SetActive(false);

		text.text = "Pull the trigger";

		remTime = limitTime;
		remTimeText.text = "time : " + remTime.ToString("N2");
		Score = 0;
		scoreText.text = "score: " + Score;
	}

	IEnumerator PlayScene()
	{
		yield return null;

		while (true)
		{
			//if (inputTriggerDown == null)
			//{
			//	Debug.Log("入力のトリガーが設定されてません");
			//	yield break;
			//}
            
			yield return StartCoroutine(WaitPullTheTrigger());

			if (isGameEnd) yield break;

			yield return StartCoroutine(WaitGameStart());

			if (isGameEnd) yield break;

			yield return StartCoroutine(PlayGame());

			if (isGameEnd) yield break;

			yield return StartCoroutine(ShowResult());

			if (isGameEnd) yield break;

			Init();

		}
	}


	IEnumerator WaitPullTheTrigger()
	{
        Debug.Log("クリックしてください ");
        //yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        gunRController.canUseGun = false;
        gunLController.canUseGun = false;

        yield return new WaitUntil(() =>
        {
            return viveRightDown;
        });
    }

	IEnumerator WaitGameStart()
    {
        Debug.Log("ゲームスタート");

        yield return null;

		timer.Initialize();

		//敵の召喚
		enemyController.InitSpawn();

		//敵が死んだらスコアが増える
		enemyController.DeathEnemy = () =>
		{
			Score++;
			Debug.Log("スコア加算: " + Score);
		};

		text.text = "3";

		Debug.Log(" 3 ");

		yield return new WaitForSeconds(1.0f);

		text.text = "2";
		Debug.Log(" 2 ");

		yield return new WaitForSeconds(1.0f);
		text.text = "1";
		Debug.Log(" 1 ");

		yield return new WaitForSeconds(1.0f);

		text.text = "Start";
		Debug.Log(" Start ");

		yield return new WaitForSeconds(2.0f);

		text.text = "";

        gunRController.canUseGun = true;
        gunLController.canUseGun = true;

    }

	IEnumerator PlayGame()
	{
		Debug.Log("ゲームスタート");
		//時間計測
		timer.Initialize();

		yield return new WaitUntil(() =>
		{
			//スコアの加算
			scoreText.text = "score: " + Score;
			timer.Update();
			remTime = limitTime - timer.time;
			remTimeText.text = "time : " + remTime.ToString("N2");

			return (0 >= remTime);
		});

		remTime = 0;
		remTimeText.text = "time : " + remTime.ToString("N2");

        //Finish
        text.text = "Finish";
        gunRController.canUseGun = false;
        gunLController.canUseGun = false;

        yield return new WaitForSeconds(3f);

		text.text = "";
		//Finish消す

		yield return null;
	}

	IEnumerator ShowResult()
	{


		//左上のUIを消す
		scoreText.gameObject.SetActive(false);
		remTimeText.gameObject.SetActive(false);

		//UI表示
		resultText.gameObject.SetActive(true);
		resultText.text = "score: " + Score;

		Debug.Log("リザルト");

        yield return new WaitForSeconds(2.0f);

        //終了
        //yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        yield return new WaitUntil(() => viveRightDown);

		resultText.gameObject.SetActive(false);

		//敵がいたら消す


		yield return null;
	}

}

