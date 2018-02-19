using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjMover : MonoBehaviour
{

    [SerializeField]
    float spd;

	void Update ()
    {
        
        if(Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(-spd,0,0);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(spd, 0, 0);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, spd, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -spd, 0);
        }


    }
}
