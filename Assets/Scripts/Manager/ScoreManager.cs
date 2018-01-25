using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    static private ScoreManager instance = null;
    static public ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = new GameObject("ScoreManager");
                var ins = obj.AddComponent<ScoreManager>();
                instance = ins;
                //Debug.Log(obj + " NullInstance");
            }
            return instance;
        }
    }
    
    void Awake()
    {
        //Debug.Log(this + " Awake");
        if(instance != null)
        {
            //Debug.Log("Destroy " + this);
            Destroy(this.gameObject);
        }
    }

    public List<Score> scores = new List<Score>();

    public Score[] GetScores() { return scores.ToArray(); }

    //現在の合計スコア
    public int GetTotalScore()
    {
        var sum = scores.Select((s) => s.point).Sum();
        Debug.Log("GetTotalScore : " + sum);

        return sum;
    }

    public void AddScore(int times, int point)
    {
        var score = new Score();
        score.name = times + "回目";
        score.point = point;
        score.times = times;
        scores.Add(score);
    }

    public void CreateScore(int num)
    {
        scores = new List<Score>();
        for(int i = 0; i < num; i++)
        {
            var score = new Score();
            score.name = i+1 + "回目";
            score.point = 0;
            scores.Add(score);
        }
    }

    public void Overwrite( int n, int times, int point )
    {
        var score = new Score();
        score.name = times + "回目";
        score.point = point;
        scores[n] = score;
    }

    public void ClearScore()
    {
        scores = new List<Score>();
    }

}
