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
    public Rigidbody Rig { get { return rig; } }

    [SerializeField]
    GameObject head;
    public GameObject Head { get { return head; } }

    [SerializeField]
    GameObject tail;
    public GameObject Tail { get { return tail; } }

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

    public bool isFarstHit;
    public int hitFlameCount;

    public bool isHitTarget;
    public bool isHitWall;
    public GameObject hitTargetObject;
    public GameObject hitWallObject;

    public System.Action<string> HitCall;

    public void Awake()
    {
        rig.isKinematic = true;
        rig.useGravity = false;
        var pos = head.transform.localPosition;
        rig.centerOfMass = pos * 0.4f;

        isHitTarget = false;
        isHitWall = false;
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

        //衝突したオブジェが同フレームでぶつかった場合
        if (isFarstHit) hitFlameCount++;
        if(hitFlameCount == 1 )
        {
            Debug.Log("Play SE from Arrow3");
            if(isHitTarget)
            {
                //範囲内であれば
                var score = hitTargetObject.GetComponent<ScoreCalculation>();
                int point = score.getScore(this.gameObject);

                Debug.Log("点P  :" + gameObject.transform.position);
                Debug.Log("点数 :" + point);

                if (point > 0)
                {

                    AudioManager.Instance.PlaySE("的に当たる");
                    Debug.Log("Played SE: 的に当たる");
                    rig.velocity = Vector3.one;
                }
                else
                {
                    //AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
                }

            }
            else //if (isHitWall)
            {
                //AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
            }
        }

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

        //※矢の風を切る音
        //AudioManager.Instance.PlaySE("");
        //Destroy(gameObject, 10f);

    }

    public void SetPosFromTail(Vector3 tailPos)
    {
        //弓のサイズの半分前に
        //Debug.Log("tail scale: " + tail.transform.lossyScale);
        var scl = tail.transform.lossyScale;
        transform.position = tailPos + transform.forward * scl.y * 0.4f ;
    }

    //移動方向を向く
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
        
        if (hitFlameCount == 0)
        {
            if (col.gameObject.tag == "Target")
            {
                isHitTarget = true;
                hitTargetObject = col.gameObject;
            }
            else if (col.gameObject.tag == "Wall")
            {
                isHitWall = true;
                hitWallObject = col.gameObject;
            }
        }

        if (isFarstHit) return;

        Debug.Log("Arrow hit Coll :" + col.transform.position);
        Debug.Log("Arraw hit : " + col.gameObject.name);

        //矢が風を切る音を止める
        //AudioManager.Instance.StopSE("");

        HitCall(col.gameObject.tag);

        //衝突したら物理挙動
        rig.useGravity = true;
        rig.mass = 0.002f;
        useLockVel = false;
        useCalc = false;

        // v = v0 + gt
        var accel = calcData.dir * calcData.speed + new Vector3(0, -calcData.grav, 0) * elapsedTime;
        accel.y = -accel.y;

        //Debug.Log("reflect :" + accel);
        //if(col.gameObject.name != "Target") rig.AddForce(accel, ForceMode.Acceleration);
        isFarstHit = true;

        //衝突SE
        //if (col.gameObject.tag == "Target") AudioManager.Instance.PlaySE("弓矢・矢が刺さる01");
        //else if (col.gameObject.tag == "Wall") AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");

    }

}
