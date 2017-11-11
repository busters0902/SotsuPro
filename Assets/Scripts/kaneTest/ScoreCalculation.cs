using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculation : MonoBehaviour {

	int getScore(GameObject arrow)
    {
        var dis = Vector3.Distance(arrow.transform.position,transform.position);
        for(int i = 10; i >= 0; i--)
        {
            if(dis > i * di * transform.localScale.x)
            {
                return 10 - i;
            }
        }
        return 0;
    }
    [SerializeField]
    float di = 0.046f;

    [SerializeField]
    GameObject obj;
    void Update()
    {
        //Debug.Log(getScore(obj));
    }

}
