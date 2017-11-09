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
		//データマネージャーの生成
		Debug.Log("_Initialize");
        //GameObject obj = new GameObject("Manager", typeof(DataManager));
        //GameObject.DontDestroyOnLoad(obj);

        //フェードマネージャーの作成
        //var fadeObj = (GameObject)Instantiate(Resources.Load("Prefabs/FadeManager"));//(GameObject)Resources.Load("Prefabs/FadeManager");
        //DontDestroyOnLoad(fadeObj);



		//Application.targetFrameRate = asset.targetFps;
		Debug.Log("target fps: " + Application.targetFrameRate);
	}

}
