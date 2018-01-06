using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTargetDirection : MonoBehaviour
{

    public GameObject target;

    public Vector3 TargetVec { get; private set; }

    public bool useUpdate = false;

    private float dotDist;

    private Vector3 dotPos;

    void Update()
    {
        if (useUpdate) Update_();
    }


    public void Update_()
    {

        //ない席から方向を求める
        var tgtVec = target.transform.position - transform.position;
        var forward = transform.forward;

        dotDist = Vector3.Dot(tgtVec, forward);
        dotPos = transform.position + forward * dotDist;

        //向きと距離 dir , dist(magnitude,length)
        TargetVec = target.transform.position - dotPos;

    }



}
