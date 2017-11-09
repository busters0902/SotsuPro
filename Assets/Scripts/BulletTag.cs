using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTag : MonoBehaviour
{

    [SerializeField]
    public Rigidbody rig;

    public void Stop()
    {

        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;

        rig.useGravity = false;

        Debug.Log("Stop");
    }


}
