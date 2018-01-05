using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//親のオブジェクトを
public class burekeigen : MonoBehaviour {
    Quaternion prevQuat;

    Quaternion thisQuat;

    // Use this for initialization
    void Start () {
        prevQuat = transform.parent.rotation;

    }
	
	// Update is called once per frame
	void Update () {
        var angle = Quaternion.Angle(transform.rotation, prevQuat);
        //Debug.Log(angle);

        if(angle > 1)
        {
            transform.localRotation= Quaternion.identity; 
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, prevQuat, 0.5f);
        }

        prevQuat = transform.rotation;
    }

    void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
    }
}
