using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTransform : MonoBehaviour
{

    public Transform target;

    public Vector3 distance;

    public bool onUsed;

    private void Update()
    {

        if(onUsed)  transform.position = target.transform.position + distance;

    }

}
