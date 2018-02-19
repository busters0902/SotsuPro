using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//データ管理
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

        roundScore = RoundScore.Create(6);

    }

    //ゲーム設定
    public GameSettings settings;

    //スコアデータ
    public RoundScore roundScore;

    //ランキングデータ
    //public ScoreRankingData[] scoreRanking;

    public GameSaveData data;

    //使わない
    public void LoadData2()
    {
        Debug.Log("ロード実行");
        var asset = Resources.Load<SettingAsset>("save_test");
        //Debug.Log(asset.scoreRanking[0].sumPoint);
        //scoreRanking = asset.scoreRanking;

    }

    //外部セーブデータがないとき
    public void CreateSaveData()
    {
        Debug.Log("PlayerPrefs データ生成");
        Utility.SetObject("SaveData", GameSaveData.Create());
    }

    public void LoadData()
    {
        Debug.Log("PlayerPrefs ロード");
        data = Utility.GetObject<GameSaveData>("SaveData");

        Debug.Log("ランキング ");
        foreach(var i in data.ranking)
        {
            Debug.Log(i.name + " : " +  i.sumPoint );
        }
    }

    public void SaveData()
    {
        
        Debug.Log("PlayerPrefs セーブ");
        Utility.SetObject("SaveData", data);

    }

    public bool IsRankIn(int point)
    {
        return data.ranking.Any( (s) => s.sumPoint < point );
    }

    public void AddRanking(ScoreRankingData s)
    {

        if (data.ranking == null) Debug.Log("rankingデータがありません 1" );
        var len = data.ranking.Length;
        if (len <= 0) Debug.Log("rankingデータがありません 2");

        data.ranking[len - 1] = s;
        data.ranking.OrderByDescending( (sc) => sc.sumPoint );

    }

}

[System.Serializable]
public class GameSaveData
{

    const int DEFAULT = 100;

    public ScoreRankingData[] ranking;

    public GameSettings settings;

    public static GameSaveData Create(int n = DEFAULT)
    {
        var obj = new GameSaveData();
        obj.ranking = new ScoreRankingData[n];
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

    public static ScoreRankingData Create(int id_, string name_, int point_)
    {
        var obj = new ScoreRankingData();
        obj.id = id_;
        obj.name = name_;
        obj.sumPoint = point_;
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

    public void DebugLog()
    {
        Debug.Log("ScoreRankingData \nid : "+ id + "\nname" + name + "\npoint" + sumPoint);
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
