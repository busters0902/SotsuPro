using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTotal : MonoBehaviour {

    int totalScore = 0;
    Text text;
    
    public void addScore(int num)
    {
        totalScore += num;
        text.text = "Total : " + totalScore.ToString();
    }
    // Use this for initialization
    void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
