using UnityEngine;
using System.Collections;

public class _Initialize : MonoBehaviour
{

	/// <summary>
	/// ゲーム起動時にシーンが呼ばれる前に一度だけ呼ばれるメソッド
	/// Managerの生成
	/// </summary>
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void Initialize()
	{

        Debug.Log("_Initialize");

        ///マネージャーの生成/////////////////////////////////////////////////

        //フェードマネージャーの作成
        //var fadeObj = (GameObject)Instantiate(Resources.Load("Prefabs/FadeManager")); //(GameObject)Resources.Load("Prefabs/FadeManager");
        //DontDestroyOnLoad(fadeObj);

        //オーディオマネージャー
        var audio = AudioManager.Instance.gameObject;
        DontDestroyOnLoad(audio);

        //データマネージャー
        var data = DataManager.Instance.gameObject;
        DontDestroyOnLoad(data);

        //スコアマネージャー
        var score = ScoreManager.Instance.gameObject;
        DontDestroyOnLoad(score);

        //Application.targetFrameRate = asset.targetFps;
        Debug.Log("target fps: " + Application.targetFrameRate);
	}

}
