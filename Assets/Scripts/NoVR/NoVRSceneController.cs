using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoVRSceneController : MonoBehaviour
{


	public bool isGameEnd;

	Timer timer;

	[SerializeField, Tooltip("残り時間")]
	float limitTime;

	float remTime;

	public int  Score { get; private set;}

	// Pull the trigger
	//3,2,1,start
	//30秒
	//Finish
	//Score

	//Press, 3, 2, 1, startなど
	[SerializeField]
	Text text;

	[SerializeField, Tooltip("残り時間")]
	Text remTimeText;

	[SerializeField]
	Text scoreText;

	[SerializeField]
	Text resultText;

	[SerializeField]
	DebugEnemyCreator enemyCreator;

	void Start()
	{
		StartCoroutine(PlayScene());

		timer = new Timer();
		timer.Initialize();
		//timer.time
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			Score++;
		}

	}

	void Init()
	{
		text.gameObject.SetActive(true);
		remTimeText.gameObject.SetActive(true);
		scoreText.gameObject.SetActive(true);
		resultText.gameObject.SetActive(false);

		text.text = "Pull the trigger";

		remTime = limitTime;
	}

	IEnumerator PlayScene()
	{
		while (true)
		{

			yield return StartCoroutine(WaitPullTheTrigger());

			if (isGameEnd) yield break;

			yield return StartCoroutine(WaitGameStart());

			if (isGameEnd) yield break;

			yield return StartCoroutine(PlayGame());

			if (isGameEnd) yield break;

			yield return StartCoroutine(ShowResult());

			if (isGameEnd) yield break;

		}
	}


	IEnumerator WaitPullTheTrigger()
	{
		Init();

	    yield return new WaitUntil( () => Input.GetMouseButtonDown(0));
		Debug.Log("pullTheTrigger click Mouse");

	}

	IEnumerator WaitGameStart()
	{
		yield return null;

		timer.Initialize();

		//敵の召喚

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
		//UIの変更

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
			remTimeText.text = "time : " + remTime.ToString("N2") ;

			return (timer.time > remTime);
		} );
		remTime = 0;

		//Finish
		text.text = "Finish";
		Debug.Log("ゲーム終了");

		yield return new WaitForSeconds(3f);

		text.text = "";
		//Finish消す

		yield return null;
	}

	IEnumerator ShowResult()
	{
		//UI表示

		//yield return new WaitForSeconds(1);

		resultText.gameObject.SetActive(true);
		resultText.text = "score: " + Score;
 
		Debug.Log("リザルト");

		//終了
		yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

		resultText.gameObject.SetActive(false);

		yield return null;
	}


}
