using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTransform : MonoBehaviour
{

    public Transform target;

    public Vector3 distance;

    public bool onUsed;

    float height = 0.3f;

    private void Update()
    {

        if (target != null)
        {
            if (onUsed)
            {
                transform.position = target.transform.position + distance;
                if(transform.position.y < height)
                {
                    transform.position = new Vector3(transform.position.x, height, transform.position.z);
                }
                transform.LookAt(target.transform.position);
            }
        }
        


    }

    void OnTriggerExit(Collider other)
    {
        string layerName = LayerMask.LayerToName(other.gameObject.layer);

        if (layerName == "CameraArea")
        {
            onUsed = false;
        }
    }
    BoxCollider boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    

    void AreaRestriction()
    {

    }


}
