using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RankingPanel: MonoBehaviour
{

    const int RANKING_NUM = 10;

    //
    List<ScoreRankingData> ranking;

    //データマネージャーから受け取る
    public void LoadRankingData()
    {
        ranking = DataManager.Instance.scoreRanking.ToList();
    }

    //※途中
    //ニュースコアをランキングの追加 
    public void AddRanking( ScoreRankingData nScore )
    {

    }

    //※途中
    //ランキングの表示
    public void ShowRanking()
    {

    }

    //※途中
    //ランキングの非表示
    public void HideRanking()
    {

    }

}
