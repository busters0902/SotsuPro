using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadeControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
        material = gameObject.GetComponent<Renderer>().material;
        FadeStart();

    }

    void FadeStart(Action callback = null)
    {
        StartCoroutine(Easing.Tween(3, (t) => {
            material.SetFloat("_Alpha", t);
        },()=> {

            StartCoroutine(Easing.Tween(3,(t)=> {
                material.SetFloat("_Alpha", 1 - t);
            }));
            if (callback != null)
                callback();
        }));
        material.SetFloat("_Alpha", 1.0f);
    }

    private Material material;
    // Update is called once per frame
    void Update () {

    }

    
}
