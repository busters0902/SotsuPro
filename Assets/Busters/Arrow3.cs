using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Calculated Arrow
[System.Serializable]
public class CalculatedData
{
    public Vector3 startPos;
    public Vector3 dir;
    public float speed;
    public Vector3 vel;
    public float grav;

    public static CalculatedData Create(Vector3 dir, float speed, float grav = 0)
    {
        var obj = new CalculatedData();
        obj.startPos = Vector3.up;
        obj.dir = dir;
        obj.speed = speed;
        obj.vel = dir * speed;
        obj.grav = grav;
        return obj;
    }

    public Vector3 GetMovedPos(float time)
    {
        //y = 1/2gt^2
        float y = 0.5f * grav * time * time;
        //xz = v0t
        Vector3 xz = dir * speed * time;
        var pos = xz + Vector3.down * y + startPos;

        //Debug.Log("Moved: " + pos);

        return pos;
    }
}


public class Arrow3 : MonoBehaviour
{

    [SerializeField]
    Rigidbody rig;

    [SerializeField]
    GameObject head;

    [SerializeField]
    GameObject tail;

    [SerializeField]
    bool useLockVel;

    public CalculatedData calcData;

    float startTime;

    float elapsedTime;

    [SerializeField]
    bool useCalc;

    public Vector3 curPos;
    public Vector3 prevPos;

    public float angleLerp;

    bool isFarstHit;

    public void Start()
    {
        rig.isKinematic = true;
        var pos = head.transform.localPosition;
        rig.centerOfMass = pos * 0.4f;
    }

    private void Update()
    {
        elapsedTime = Time.time - startTime;
        
        if (useCalc)
        {
            var pos = MoveCalcedPosition(elapsedTime);
            prevPos = curPos;
            curPos = pos;
            //transform.position = pos;
            rig.MovePosition(pos);
        }

        if (useLockVel) LookVelocity();
    }

    public void Shot(CalculatedData data)
    {
        if (data == null) Debug.LogError("data is null");

        startTime = Time.time;
        calcData = data;

        rig.isKinematic = false;
        useCalc = true;
        useLockVel = true;
        curPos = prevPos = transform.position;

        Destroy(gameObject, 10f);
    }

    public void SetPosFromTail(Vector3 tailPos)
    {
        //弓のサイズの半分前に
        Debug.Log("tail scale: " + tail.transform.lossyScale);
        var scl = tail.transform.lossyScale;
        transform.position = tailPos + transform.up * scl.y;
    }

    //移動法を向く
    public void LookVelocity()
    {
        if (!rig.isKinematic)
        {
            var vel = curPos - prevPos;
            if (vel != Vector3.zero)
            {
                //rig.MoveRotation(Quaternion.LookRotation(vel));
                var qt = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(vel), angleLerp);
                rig.MoveRotation(qt);
            }
        }
    }

    public Vector3 MoveCalcedPosition(float time)
    { 
        return calcData.GetMovedPos(time);
    }

    public void Stick()
    {

    }

    public void OnCollisionEnter(Collision col)
    {

        if (isFarstHit) return;

        //衝突したら物理挙動
        rig.useGravity = true;
        rig.mass = 0.002f;
        useLockVel = false;
        useCalc = false;

        // v = v0 + gt
        var accel = calcData.dir * calcData.speed + new Vector3(0, -calcData.grav, 0) * elapsedTime;
        accel.y = -accel.y;
        Debug.Log(accel);

        rig.AddForce( accel, ForceMode.Acceleration);
        isFarstHit = true;

    }

}
