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

        }
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return))
        {
         
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Return))
        {
            bow.Shoot();
            isSettingArrow = false;
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

        if(Input.GetKeyDown(KeyCode.Alpha1))
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
