using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DebugAPScene : MonoBehaviour
{

    void Update()
    {
        //現在のデータをセーブ
        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Debug Save SaveData");
            DataManager.Instance.SaveData();
        }

        //データを生成
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Debug Create SaveData");
            DataManager.Instance.data = GameSaveData.Create();
            DataManager.Instance.SaveData();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            //DataManager.Instance.data.ranking.OrderByDescending((s) => s.sumPoint );
            //DataManager.Instance.data.ranking.OrderBy((s) => s.sumPoint);
            var r = DataManager.Instance.data.ranking;
            DataManager.Instance.data.ranking = r.OrderByDescending((s) => s.sumPoint).ToArray();
            //DataManager.Instance.data.ranking = DataManager.Instance.data.ranking.OrderByDescending((s) => s.sumPoint).ToArray();
            Debug.Log("Debug ソートします "); 
        }

    }

}

