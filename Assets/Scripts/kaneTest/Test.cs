using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //FadeControl.Instance.FadeStart();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //AudioManager.Instance.PlaySE("引き絞り");
            AudioManager.Instance.PlaySeList("Test");
            
        }


    }
}
