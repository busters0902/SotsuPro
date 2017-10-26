using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField]
    Rigidbody rig;

    [SerializeField]
    GameObject tail;

    public void Shot( Vector3 dir, float power )
    {
        rig.useGravity = true;
        rig.velocity = dir * power;

        Destroy(gameObject, 10f);
    }

}
