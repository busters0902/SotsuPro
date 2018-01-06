using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bustersSceneController : MonoBehaviour
{

    [SerializeField]
    Bow2 bow;

    [SerializeField]
    PredictionLine preLine;

    bool isSettingArrow;

    [SerializeField]
    ResultController result;

    [SerializeField]
    RankingController ranking;

    Vector3 currMousePos;
    Vector3 prevMousePos;

    //SEが鳴るまで一定の距離
    [SerializeField]
    float soundLengthLimit;

    //現在の距離
    float soundLength;

    void Start()
    {
        preLine.CreateLine();
        UpdateLine();
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            bow.CreateArrow();
            isSettingArrow = true;
            UpdateLine();

            currMousePos = Input.mousePosition;
            prevMousePos = currMousePos;
            //Debug.Log(currMousePos);

        }
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return))
        {
            prevMousePos = currMousePos;
            currMousePos = Input.mousePosition;
            var mov = Mathf.Abs(prevMousePos.magnitude - currMousePos.magnitude);
            soundLength += mov;
            if(soundLength >= soundLengthLimit)
            {
                //※音を鳴らす
                soundLength = 0.0f; //リセット
                AudioManager.Instance.PlaySE("引き絞り3");
                Debug.Log("ギィ");
            }
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Return))
        {
            bow.Shoot();
            isSettingArrow = false;

            prevMousePos = Vector3.zero;
            currMousePos = Vector3.zero;
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            bow.transform.Rotate(-1,0,0);
            UpdateLine();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            bow.transform.Rotate(1, 0, 0);
            UpdateLine();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            bow.transform.Rotate(0, -1, 0);
            UpdateLine();
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            bow.transform.Rotate(0, 1, 0);
            UpdateLine();
        }


        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            AudioManager.Instance.ShowSeNames();
        }

        ///サウンドテスト/////////////////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.PlaySE("引き絞り");
            Debug.Log("引き絞り");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.Instance.PlaySE("弦引き");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.Instance.PlaySE("的に当たる");
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AudioManager.Instance.PlaySE("矢の飛来");
        }

        var keyTest = KeyCode.Alpha6;
        if (Input.GetKeyDown(keyTest))
        {
            AudioManager.Instance.PlaySELoop("テスト","引き絞り2");
        }
        else if(Input.GetKeyUp(keyTest))
        {
            AudioManager.Instance.StopSELoop("テスト");
        }


        ///リザルトテスト/////////////////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            //result.HideAll();
            ranking.HideRanking();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //result.ShowResult( () => Debug.Log("リザルト＼(^o^)／ｵﾜﾀ") );
            ranking.ShowRanking();
        }


        ///セーブテスト/////////////////////////////////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.R))
        {
            //DataManager.Instance.LoadData();
            DataManager.Instance.CreateSaveData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            //DataManager.Instance.LoadData();
            DataManager.Instance.LoadData();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //DataManager.Instance.SaveData();
            DataManager.Instance.SaveData();
        }


    }

    void UpdateLine()
    {
        var data = new CalculatedData();
        data.dir = bow.transform.forward;
        data.speed = bow.curPower;
        if( isSettingArrow) data.startPos = bow.arrow.transform.position;
        else    data.startPos = bow.transform.position;
        data.grav = bow.arrowGrav;
        preLine.CalcLine(data);
    }


}
