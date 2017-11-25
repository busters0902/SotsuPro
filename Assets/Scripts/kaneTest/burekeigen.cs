using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class burekeigen : MonoBehaviour {
    Quaternion prevQuat;
	// Use this for initialization
	void Start () {
        prevQuat = transform.parent.rotation;

    }
	
	// Update is called once per frame
	void Update () {

         var angle = 
        Quaternion.Angle(transform.rotation, prevQuat);

        if(angle > 1)
        {
            transform.rotation = Quaternion.identity; 
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, prevQuat, 0.9f);
        }
            Debug.Log(angle);


        prevQuat = transform.parent.rotation;
    }
}
