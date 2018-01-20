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

        //オーディオマネージャー
        var audio = AudioManager.Instance.gameObject;
        DontDestroyOnLoad(audio);

        //データマネージャー
        var data = DataManager.Instance.gameObject;
        DontDestroyOnLoad(data);

        //データのロード
        DataManager.Instance.LoadData();

        //スコアマネージャー
        var score = ScoreManager.Instance.gameObject;
        DontDestroyOnLoad(score);

        //Application.targetFrameRate = asset.targetFps;
        Debug.Log("target fps: " + Application.targetFrameRate);
	}

}
