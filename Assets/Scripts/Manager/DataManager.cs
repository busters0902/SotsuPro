using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static private DataManager instance = null;
    static public DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("DataManager");
                var ins = obj.AddComponent<DataManager>();
                instance = ins;
            }
            return instance;
        }
    }

    void Awake()
    {
        //Debug.Log(this + " Awake");
        if (instance != null)
        {
            //Debug.Log("Destroy " + this);
            Destroy(this.gameObject);
        }
    }


    void Start()
    {
        //ゲーム設定
        settings = new GameSettings();

        //ランキング
        int rankNum = 10;
        scoreRanking = new ScoreRankingData[rankNum];

        for(int i = 0; i < rankNum; i++)
        {
            scoreRanking[i] = new ScoreRankingData();
            scoreRanking[i].id = -1;
            scoreRanking[i].name = "名無し";
            scoreRanking[i].sumPoint = -1;
        }

        roundScore = RoundScore.Create(6);

    }

    //ゲーム設定
    public GameSettings settings;

    //スコアデータ
    public RoundScore roundScore;

    //ランキングデータ
    public ScoreRankingData[] scoreRanking;

    public GameSaveData data;


    public void LoadData2()
    {
        Debug.Log("ロード実行");
        var asset = Resources.Load<SettingAsset>("save_test");
        //Debug.Log(asset.scoreRanking[0].sumPoint);
        //scoreRanking = asset.scoreRanking;

    }

    public void CreateSaveData()
    {
        Debug.Log("PlayerPrefs データ生成");
        Utility.SetObject("SaveData", GameSaveData.Create());
    }

    public void LoadData()
    {
        Debug.Log("PlayerPrefs ロード");
        data = Utility.GetObject<GameSaveData>("SaveData");
    }

    public void SaveData()
    {
        
        Debug.Log("PlayerPrefs セーブ");
        Utility.SetObject("SaveData", data);
    }


}

[System.Serializable]
public class GameSaveData
{

    public ScoreRankingData[] ranking;

    public GameSettings settings;

    public static GameSaveData Create()
    {
        var obj = new GameSaveData();
        obj.ranking = new ScoreRankingData[10];
        for (int i = 0; i < obj.ranking.Length; i++)
        {
            obj.ranking[i] = ScoreRankingData.Create();
        }
        obj.settings= GameSettings.Create();
        return obj;
    }

}

[System.Serializable]
public class ScoreRankingData
{
    public int id;
    public string name;
    public int sumPoint;

    public static ScoreRankingData Create()
    {
        var obj = new ScoreRankingData();
        return obj;
    }

    public static ScoreRankingData[] Create(int n)
    {
        var obj = new ScoreRankingData[n];
        for (int i = 0; i < obj.Length; i++)
        {
            obj[i] = ScoreRankingData.Create();
        }
        return obj;
    }
}

[System.Serializable]
public class GameSettings
{
    public string name;

    public int a;

    public float b;

    public static GameSettings Create()
    {
        var obj = new GameSettings();
        return obj;
    }
}
