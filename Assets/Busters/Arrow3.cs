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
        obj.dir = dir;
        obj.speed = speed;
        obj.vel = dir * speed;
        obj.grav = grav;
        return obj;
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
            transform.position = pos;
        }

        if (useLockVel) LockVelocity();
    }

    public void Shot(CalculatedData data)
    {
        startTime = Time.time;
        calcData = data;

        rig.isKinematic = false;
        useCalc = true;
        useLockVel = true;


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
    public void LockVelocity()
    {
        if (!rig.isKinematic)
        {
            var vel = curPos - prevPos;
            if (vel != Vector3.zero)
            {
                //rig.MoveRotation(Quaternion.LookRotation(vel));
                var qt = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(vel), 0.1f);
                rig.MoveRotation(qt);
            }
        }
    }

    //y  = vY * g * t;
    //xz = vXY * t;
    public Vector3 MoveCalcedPosition(float time)
    {
        float y = calcData.grav * time;

        Vector3 xz = calcData.dir * calcData.speed * time ;
        var pos = xz + Vector3.down * y + calcData.startPos;

        Debug.Log("Move: " + pos);

        return pos;
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

        var accel = calcData.dir * calcData.speed + (new Vector3(0, -calcData.grav, 0) * elapsedTime * elapsedTime);
        accel.y = -accel.y;
        Debug.Log(accel);

        rig.AddForce( accel, ForceMode.Acceleration);
        isFarstHit = true;

    }

}
