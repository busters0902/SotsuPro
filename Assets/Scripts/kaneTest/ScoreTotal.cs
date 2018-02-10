using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTotal : MonoBehaviour
{

    int totalScore = 0;
    public int TotalScore{ get { return totalScore;} }

    [SerializeField]
    Text text;
    
    public void AddScore(int num)
    {
        totalScore += num;
        //text.text = "Total : " + totalScore.ToString();
        text.text = totalScore.ToString();
    }

    public void ResetScore()
    {
        totalScore = 0;
        //text.text = "Total : 0"; 
        text.text = "0";
    }
}
