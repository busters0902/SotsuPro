using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//移動予測線
public class PredictionLine : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRend;

    
    //分割数
    public int lineNum;

    //何秒後まで
    public float timeLimit;

    public Vector3 iniPos;
   
    public bool useUpdate;

    public CalculatedData calcData;

    public Action<float, float, Vector3> linePostion;

    void Update()
    {
        if (useUpdate) CalcLine();
    }

    public void CreateLine()
    {
        
        Vector3[] points = new Vector3[lineNum];
        //List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < lineNum; i++)
        {
            var pos = iniPos + new Vector3(0f, 0f, 0.2f * i);
            points[i] = pos;
            //points.Add(pos);
            //Debug.Log(pos);
        }

        //先にサイズを設定
        lineRend.positionCount = lineNum;
        lineRend.SetPositions(points);   

    }

    public void CalcLine(CalculatedData data = null)
    {
        Debug.Log("Calcline");
        if(data == null)
        {
            if (calcData == null)
            {
                Debug.LogError("calcData null !");
                return;
            }
        }
        else
        {
            calcData = data;
        }
            
        //何秒間　を　何分割
        var tpl = timeLimit / (float)lineNum;

        var num = lineRend.positionCount;
        for (int i = 0; i < num; i++)
        {
            Vector3[] points = new Vector3[num];
            lineRend.GetPositions(points);
            //時間 (tpl* (float)i);
            Debug.Log(" arry " + points[i]);
            var pos = calcData.GetMovedPos(tpl * (float)i);
            points[i] = pos;
            lineRend.SetPositions(points);
        }
    }

}
