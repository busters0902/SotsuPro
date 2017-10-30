﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField]
    Rigidbody rig;

    [SerializeField]
    GameObject tail;

    public void Start()
    {
        rig.isKinematic = true;
    }

    public void Shot( Vector3 dir, float power )
    {
        rig.useGravity = true;
        rig.velocity = dir * power;
        rig.isKinematic = false;

        Destroy(gameObject, 10f);
    }

    public void SetPosFromTail(Vector3 tailPos)
    {
        //弓のサイズの半分前に
        Debug.Log("tail scale: " + tail.transform.lossyScale);
        var half = tail.transform.lossyScale;
        transform.position = tailPos + transform.up * half.y;
    }

}
