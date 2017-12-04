using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FadeControl : MonoBehaviour
{

    static FadeControl instance;
    static public FadeControl Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = Resources.Load("Prefabs/Fade/FadeQuad");
                var _instance = Instantiate(obj);
            }
            return instance;
        }
    }

    private Material material;

    void Awake()
    {
        instance = this;
        material = gameObject.GetComponent<Renderer>().material;
    }


    public void SetGemeobject(GameObject obj)
    {
        var buf_pos = transform.localPosition;
        transform.SetParent(obj.transform);
        transform.localPosition = buf_pos;
        transform.localRotation = Quaternion.identity;
    }

    public void FadeStart(Action callback = null)
    {

        StartCoroutine(Easing.Tween(1, (t) =>
        {
            material.SetFloat("_Alpha", t);
        }, () =>
        {

            StartCoroutine(Easing.Tween(1, (t) =>
            {
                material.SetFloat("_Alpha", 1 - t);
            }));
            if (callback != null)
                callback();
        }));
        material.SetFloat("_Alpha", 1.0f);
    }

    public void FadeIn(float time = 1, float dyray = 0, Action callback = null)
    {
        StartCoroutine(Easing.Deyray(dyray, () =>
        {



            StartCoroutine(Easing.Tween(time, (t) =>
            {
                material.SetFloat("_Alpha", 1 - t);
            }, callback));
        }));

    }
}
