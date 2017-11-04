using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移動予測線
public class PredictionLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRend;

    
    //分割すう
    public int lineNum;

    //何秒後まで
    public float timeLimit;

    public Vector3 iniPos;
   
    public bool useUpdate;

    public float grav;

    //public float time;

    //v0 = dir * magnitude
    public Vector3 dir;

    [SerializeField]
    float mag;

    public Action<float, float, Vector3> linePostion;

    void Update()
    {
        if (useUpdate) CalcLine();
    }

    public void CreateLine()
    {
        //line
        Vector3[] points = new Vector3[lineNum];
        //List<Vector3> points = new List<Vector3>();
        for (int i = 0; i < lineNum; i++)
        {
            var pos = iniPos + new Vector3(0f, 0f, 0.2f * i);
            points[i] = pos;
            //points.Add(pos);
            Debug.Log(pos);
        }

        //さきにサイズを入れる
        lineRend.positionCount = lineNum;
        lineRend.SetPositions(points);   

    }

    //v0 = dir * mag;
    //pos = v0*t + gt;
    //mag = 240 km/h;
    // pos = (dir*mag + g) * t;

    //vel = dir*mag + g
    //pos = vel.nomalize;
    //mag = vel.magnitude;


    //h = 60m 60s 60f;
    //h = 60m 60s deltaTime;

    //y  = gt;
    //xz = 1/2 g t^2;


    public void CalcLine()
    {
        //何秒間　を　何分割
        var tpl = timeLimit / lineNum;


        var num = lineRend.positionCount;
        for (int i = 0; i < num; i++)
        {
            Vector3[] points = null;
            lineRend.GetPositions(points);
            //tpl* (float)i;
            points[i] = new Vector3(0, 0, 0);

        }
    }

    //public Vector3 MoveCalcedPosition(float time)
    //{
    //    float y = calcData.grav * time;

    //    Vector3 xz = calcData.dir * calcData.speed * time;
    //    var pos = xz + Vector3.down * y + calcData.startPos;

    //    Debug.Log("Move: " + pos);

    //    return pos;
    //}

}
