using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTotal : MonoBehaviour {

    int totalScore = 0;
    public int TotalScore{ get { return totalScore;} }

    [SerializeField]
    Text text;
    
    public void addScore(int num)
    {
        totalScore += num;
        text.text = "Total : " + totalScore.ToString();
    }
    
    void Start () {
        //text = GetComponent<Text>();
	}

    public void ResetScore()
    {
        totalScore = 0;
        text.text = "Total : 0"; 
    }
}
