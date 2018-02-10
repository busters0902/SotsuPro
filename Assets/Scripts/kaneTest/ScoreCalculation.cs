using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculation : MonoBehaviour{

	public int getScore(GameObject arrow)
    {
        var dis = Vector2.Distance(arrow.transform.position,transform.position);
        for(int i = 10; i >= 0; i--)
        {
            if(dis > i * di * transform.localScale.x)
            {
                return 10 - i;
            }
        }
        return 0;
    }

    public int getScore(Vector3 pos)
    {
        var dis = Vector2.Distance(pos, transform.position);
        for (int i = 10; i >= 0; i--)
        {
            if (dis > i * di * transform.localScale.x)
            {
                return 10 - i;
            }
        }
        return 0;
    }

    [SerializeField]
    float di = 0.046f;

}
