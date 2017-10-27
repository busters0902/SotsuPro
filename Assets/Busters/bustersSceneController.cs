using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bustersSceneController : MonoBehaviour
{

    [SerializeField]
    Bow bow;

    [SerializeField]
    Camera camera;


    void Update()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            bow.CreateArrow();
        }
        else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return))
        {
            bow.UpdateDraw(0.01f);
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Return))
        {
            bow.Shoot();
        }

        if(Input.GetKey(KeyCode.UpArrow))
        {
            bow.transform.Rotate(-1,0,0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            bow.transform.Rotate(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            bow.transform.Rotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            bow.transform.Rotate(0, 1, 0);
        }

    }



}
