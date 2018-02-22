using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RankingController : MonoBehaviour
{

    public RankingPanel panel;

    const int RANKING_NUM = 100;
    List<ScoreRankingData> ranking;

    //データマネージャーから受け取る
    public void LoadRankingData()
    {

        ranking = DataManager.Instance.data.ranking.ToList();
        var scoreViewers = panel.rankScores;
        int i = 0;
        for (i = 0; i < ranking.Count; i++)
        {
            scoreViewers[i].logoText.text.text = ranking[i].id.ToString();
            scoreViewers[i].point.text = ranking[i].sumPoint.ToString();
        }
        for (; i < panel.rankScores.Length; i++)
        {
            scoreViewers[i].logoText.text.text = i.ToString();
            scoreViewers[i].point.text = "0";
        }

    }

    //※途中
    //ニュースコアをランキングの追加 
    public void AddRanking(ScoreRankingData nScore)
    {

    }

    [SerializeField]
    GameObject content;

    [SerializeField]
    float contentWidth;

    List<GameObject> prefabNum = new List<GameObject>();
    //※
    //ランキング生成
    public void CreateRankingPanel()
    {
        prefabNum.Clear();
        var prefab = (GameObject)Resources.Load("Prefabs/Rank1Score");
        panel.rankNumber = new Text[RANKING_NUM];
        panel.rankScores = new RankScoreViewer[RANKING_NUM];
        for (int i = 0; i < RANKING_NUM; i++)
        {
            var obj = Instantiate(prefab);
            obj.transform.SetParent(content.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);


            panel.rankNumber[i] = obj.GetComponent<LogoText>().text;
            panel.rankScores[i] = obj.GetComponent<RankScoreViewer>();
            obj.transform.Find("Rank1").GetComponent<Text>().text = (i + 1).ToString();
            prefabNum.Add(prefab);
        }

    }

    public void posReset()
    {
        RectTransform scrollRect = content.GetComponent<RectTransform>();
        var num = 29.6f / 100f;
       
        var from = num * 100;
        scrollRect.localPosition = new Vector3(scrollRect.localPosition.x,
                from, scrollRect.localPosition.y);
    } 

    public void moveContent(int rankNum, System.Action callback = null)
    {
        ////RectTransform rectTransform;
        //ScrollRect scrollRect = transform.Find("scrollvieew").GetComponent<ScrollRect>();
        ////content.GetComponent<RectTransform>().position = new Vector3(content.transform.localPosition.x, prefabNum[rankNum].GetComponent<RectTransform>().position.y, content.transform.localPosition.z);
        //scrollRect.verticalNormalizedPosition = -102000 * rankNum;

        RectTransform scrollRect = content.GetComponent<RectTransform>();
        //content.GetComponent<RectTransform>().position = new Vector3(content.transform.localPosition.x, prefabNum[rankNum].GetComponent<RectTransform>().position.y, content.transform.localPosition.z);
        //if (rankNum < 3)
        //{
        //    rankNum = 1;
        //}
        var num = 29.6f / 100f;
        //scrollRect.localPosition = new Vector3(scrollRect.localPosition.x, Mathf.Min(Mathf.Max((num * rankNum) + (num * 2.0f), 0), 29.6f - num * 2), scrollRect.localPosition.y);
        var from = num * 100;
        var to = Mathf.Min(Mathf.Max((num * (float)rankNum) + (num * 1.0f), 0.0f), 29.6f - num * 2.0f);
        StartCoroutine(
        Utility.TimeCrou(2, (t) =>
        {
            scrollRect.localPosition = new Vector3(scrollRect.localPosition.x,
                Mathf.Lerp(from, to, curve.Evaluate(t)), scrollRect.localPosition.y);
        }, () =>
        {
            StartCoroutine(
        Utility.TimerCrou(1, () =>
        {
            StartCoroutine(
        Utility.TimeCrou(5, (t) =>
        {
            scrollRect.localPosition = new Vector3(scrollRect.localPosition.x,
                Mathf.Lerp(to, 0.685f, curve.Evaluate(t)), scrollRect.localPosition.y);
        }, callback));


        }));
        }));

    }
    public
    AnimationCurve curve;
    //※途中
    //ランキングの表示
    public void ShowRanking()
    {
        for (int i = 0; i < panel.rankScores.Length; i++)
        {
            //panel.rankNumber[i].gameObject.SetActive(true);
            panel.rankScores[i].gameObject.SetActive(true);
        }
    }

    //※途中
    //ランキングの非表示
    public void HideRanking()
    {
        for (int i = 0; i < panel.rankScores.Length; i++)
        {
            //panel.rankNumber[i].gameObject.SetActive(false);
            panel.rankScores[i].gameObject.SetActive(false);
        }
    }

    IEnumerator HideRinkingCrou()
    {

        yield return null;
    }

}