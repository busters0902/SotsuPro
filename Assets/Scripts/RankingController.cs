using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RankingController : MonoBehaviour
{

    public RankingPanel rankingPanel;

    const int RANKING_NUM = 10;
    List<ScoreRankingData> ranking;

    //データマネージャーから受け取る
    public void LoadRankingData()
    {
        ranking = DataManager.Instance.data.ranking.ToList();
        var scoreViewers = rankingPanel.rankScores;
        for (int i = 0; i < rankingPanel.rankScores.Length; i++)
        {
            scoreViewers[i].logoText.text.text = ranking[i].sumPoint.ToString();
        }

    }

    //※途中
    //ニュースコアをランキングの追加 
    public void AddRanking(ScoreRankingData nScore)
    {

    }

    //※途中
    //ランキングの表示
    public void ShowRanking()
    {
        for(int i = 0; i < rankingPanel.rankScores.Length; i++)
        {
            rankingPanel.rankNumber[i].gameObject.SetActive(true);
            rankingPanel.rankScores[i].gameObject.SetActive(true);
        }
    }

    //※途中
    //ランキングの非表示
    public void HideRanking()
    {
        for (int i = 0; i < rankingPanel.rankScores.Length; i++)
        {
            rankingPanel.rankNumber[i].gameObject.SetActive(false);
            rankingPanel.rankScores[i].gameObject.SetActive(false);
        }
    }

    IEnumerator HideRinkingCrou()
    {

        yield return null;
    }

}
