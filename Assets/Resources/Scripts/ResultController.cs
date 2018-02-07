using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultController : MonoBehaviour
{

    [SerializeField]
    public ResultPanel panel;

    public Score[] scores;

    bool onHideAll;
    bool onShowResult;

    void Start()
    {
        onHideAll = false;
        onShowResult = false;


    }

    //初期化
    public void Initialize()
    {
        Reset_();
    }

    public void Load()
    {
        //scores = DataManager.Instance.roundScore.scores;
        scores = ScoreManager.Instance.scores.ToArray();
    }


    public void HideAll()
    {
        if (onShowResult) return;

        Debug.Log("HideAll");
        foreach (var t in panel.timesTexts)
        {
            t.gameObject.SetActive(false);
        }
        foreach (var t in panel.pointTexts)
        {
            t.gameObject.SetActive(false);
        }
        
        panel.parse.gameObject.SetActive(false);
        panel.sumText.gameObject.SetActive(false);
        panel.sumPointText.gameObject.SetActive(false);

    }

    public void ShowResult()
    {
        StartCoroutine(ShowResultCrou());
    }

    public IEnumerator ShowResultCrou()
    {
        onShowResult = true;

        var interval = 0.3f;

        yield return null;

        for(int i = 0; i < panel.timesTexts.Length; i++ )
        {
            panel.timesTexts[i].text = scores[i].times + "回目";
            panel.pointTexts[i].text = scores[i].point.ToString();

            panel.timesTexts[i].gameObject.SetActive(true);
            panel.pointTexts[i].gameObject.SetActive(true);

            yield return new WaitForSeconds(interval);

        }

        panel.parse.gameObject.SetActive(true);

        yield return new WaitForSeconds(interval);

        var sum = scores.Select(d => d.point).Sum();
        panel.sumPointText.text = sum.ToString();

        panel.sumText.gameObject.SetActive(true);
        panel.sumPointText.gameObject.SetActive(true);

        onShowResult = false;
    }

    public void ShowResult(System.Action callback)
    {
        StartCoroutine(ShowResultCrou(callback));
    }

    public IEnumerator ShowResultCrou(System.Action callback)
    {
        yield return ShowResultCrou();

        callback();
    }

    public void Reset_()
    {
        scores = new Score[10];

        HideAll();
    }



}
