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
    public Vector3 nextPos;

    public float angleLerp;

    public bool isFarstHit;
    public int hitFlameCount;

    public bool isHitTarget;
    public bool isHitWall;
    public GameObject hitTargetObject;
    public GameObject hitWallObject;

    public System.Action<string> ShotCall;
    public System.Action<string> HitCall;

    [SerializeField]
    ParticleSystem windParticle;

    public bool useCalcIntersect;
    public List<QuadCollider> targets = new List<QuadCollider>();


    public void Awake()
    {
        rig.isKinematic = true;
        rig.useGravity = false;
        var pos = head.transform.localPosition;
        rig.centerOfMass = pos * 0.4f;

        isHitTarget = false;
        isHitWall = false;

        InitHitCall();
    }

    private void Update()
    {
        elapsedTime = Time.time - startTime;

        //移動計算
        if (useCalc)
        {
            var pos = MoveCalcedPosition(elapsedTime);
            prevPos = curPos;
            curPos = pos;
        }

        //衝突判定計算
        if (useCalcIntersect)
        {
            //ターゲットの判定
            Vector3 cross = new Vector3();
            //var hit = UtilityCollision.IntersectSegmentQuadrangle(prevPos, curPos, targets[0].col, ref cross);
            var hit = UtilityCollision.IntersectSegmentCircle(prevPos, curPos, targets[0].col, targets[0].rad, ref cross);

            //ターゲットに衝突
            if (hit)
            {
                Debug.Log("Hit IntersectSegment :" + cross);
                Stop();
                rig.isKinematic = true;
                useCalcIntersect = false;
                isHitTarget = true;
                SetPosFromHead(cross);
                useCalc = false;
                windParticle.Stop();

            }

        }

        if (useCalc)
        {
            rig.MovePosition(CalcPosFromHead(curPos));
        }

        if (useLockVel) LookVelocity();

        //衝突したオブジェが同フレームでぶつかった場合
        if (isFarstHit) hitFlameCount++;
        if (hitFlameCount == 1)
        {
            Debug.Log("Play SE from Arrow3");
            if (isHitTarget)
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
                    AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");
                }

                ScoreManager.Instance.AddScore(0, point);

            }
            else
            {
                AudioManager.Instance.PlaySE("弓矢・矢が刺さる03");

                ScoreManager.Instance.AddScore(0, 0);
            }

        }

    }

    //打ったときのメソッド
    public void Shot(CalculatedData data)
    {
        if (data == null) Debug.LogError("data is null");

        startTime = Time.time;
        calcData = data;

        rig.isKinematic = false;
        useCalc = true;
        useLockVel = true;
        curPos = prevPos = transform.position;

        windParticle.Play();
    }

    //的に当たったときの処理
    public void HitStopCall()
    {
        windParticle.Stop();
    }

    public void SetPosFromTail(Vector3 tailPos)
    {
        //弓のサイズの半分前に
        //Debug.Log("tail scale: " + tail.transform.lossyScale);
        var scl = tail.transform.lossyScale;
        transform.position = tailPos + transform.forward * scl.y * 0.4f;
    }

    public void SetPosFromHead(Vector3 headPos)
    {
        //弓のサイズの半分前に
        //Debug.Log("tail scale: " + tail.transform.lossyScale);
        var scl = head.transform.lossyScale;
        transform.position = headPos - transform.forward * 0.4f;
    }

    public Vector3 CalcPosFromHead(Vector3 headPos)
    {
        var scl = head.transform.lossyScale;
        return 　headPos - transform.forward * scl.y * 0.4f;
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

        Debug.Log("Arraw hit : " + col.gameObject.name);

        //矢が風を切る音を止める
        windParticle.Stop();
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

    public void InitHitCall()
    {
        HitCall = (s) => Debug.Log(s);
    }

    public void Stop()
    {
        rig.mass = 0.002f;
        useLockVel = false;
        useCalc = false;
    }
}
