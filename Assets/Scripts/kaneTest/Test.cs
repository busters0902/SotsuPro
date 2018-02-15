using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //FadeControl.Instance.FadeStart();
	}
    [SerializeField]
    Transform target;

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            //AudioManager.Instance.PlaySE("引き絞り");
            //AudioManager.Instance.PlaySeList("Test");
            
        }
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    AudioManager.Instance.PlaySE("gaya", target.localPosition);

        //}
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    AudioManager.Instance.FadeOutSE("gaya",0.5f);

        //}


    }
}
