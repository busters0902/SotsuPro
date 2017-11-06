using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bustersSceneController : MonoBehaviour
{

    [SerializeField]
    Bow bow;

    [SerializeField]
    PredictionLine preLine;

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
            //preLine.CalcLine(bow);
            

        }
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return))
        {
         
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Return))
        {
            bow.Shoot();
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

    }

    void UpdateLine()
    {
        var data = new CalculatedData();
        data.dir = bow.transform.forward;
        data.speed = bow.curPower;
        data.startPos = bow.transform.position;
        data.grav = bow.arrowGrav;
        preLine.CalcLine(data);
    }


}
