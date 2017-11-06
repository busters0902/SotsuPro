using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingArea : MonoBehaviour
{

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("OnCollision Enter");      
        if(col.gameObject.tag == "Bullet" )
        {
            Debug.Log("Bullet Hit");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log("On Trigger");
        if (col.gameObject.tag == "Bullet")
        {
            Debug.Log("Hit " + col.gameObject);
            var rigAc = col.gameObject.GetComponent<RigAccess>();
            rigAc.rig.velocity = Vector3.zero;
            rigAc.rig.useGravity = false;
        }
    }


}
