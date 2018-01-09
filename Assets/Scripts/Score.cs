using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score
{
    public string name;
    public int times;
    public int point;
}

[System.Serializable]
public class RoundScore
{
    public string name;
    public int round;
    public Score[] scores;

    static public RoundScore Create(int times)
    {
        var obj = new RoundScore();
        obj.name = "";
        obj.round = 0;
        obj.scores = new Score[times];
        return obj;
    }
}