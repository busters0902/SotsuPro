using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityLineMover : MonoBehaviour
{

    [SerializeField]
    Rigidbody rig;

    [SerializeField]
    bool useMode;

    [SerializeField]
    

    void ChangeMode()
    {

    }

    void UseGravity(bool f)
    {
        rig.useGravity = f;
    }



}
