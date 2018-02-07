using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandAnim : MonoBehaviour
{

    [SerializeField]
    Animator anim;

    //enum HandState


    public void Catch()
    {
        anim.SetTrigger("catch");
    }

    public void Release()
    {
        anim.SetTrigger("release");
    }

}
