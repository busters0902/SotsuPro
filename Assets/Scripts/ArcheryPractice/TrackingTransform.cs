using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTransform : MonoBehaviour
{

    public Transform target;

    public Vector3 distance;

    private void Update()
    {
        transform.position = target.transform.position + distance;
    }

}
