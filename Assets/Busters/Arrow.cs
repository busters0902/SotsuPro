using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField]
    Rigidbody rig;

    [SerializeField]
    GameObject tail;

    [SerializeField]
    GameObject head;

    [SerializeField]
    bool useLockVel;

    public void Start()
    {
        rig.isKinematic = true;
        var pos = head.transform.localPosition;
        rig.centerOfMass = pos * 0.4f;
    }

    private void Update()
    {
        if(useLockVel) LockVelocity();
    }

    public void Shot(Vector3 dir, float power)
    {
        //rig.useGravity = true;
        rig.velocity = dir * power;
        rig.isKinematic = false;

        Destroy(gameObject, 10f);
    }

    public void SetPosFromTail(Vector3 tailPos)
    {
        //弓のサイズの半分前に
        Debug.Log("tail scale: " + tail.transform.lossyScale);
        var scl = tail.transform.lossyScale;
        transform.position = tailPos + transform.up * scl.y;
    }

    public void LockVelocity()
    {
        if (!rig.isKinematic)
        {
            var vel = rig.velocity;
            var tmp = vel.x;
            if (rig.velocity != Vector3.zero)
            {
                //rig.MoveRotation(Quaternion.LookRotation(rig.velocity));
                var qt = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(rig.velocity), 0.4f);
                rig.MoveRotation(qt);
            }
        }
    }

    public void Stick()
    {

    }

    public void OnCollisionEnter(Collision col)
    {
        rig.useGravity = true;
        rig.mass = 0.002f;
        useLockVel = false;
    }
}
