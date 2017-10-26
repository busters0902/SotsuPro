using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SingletonManager : MonoBehaviour
{ 
	public static SingletonManager Instance{ get; private set; }

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			//DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this);
			return;
		}

		Debug.Log("Awake : " + gameObject.ToString());
	}

	public void DontDestroy()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
