using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Stance スタンス（足踏み）
Set セット（胴造り）
Nocking ノッキング（矢つがえ）
Set up セットアップ（打ちおこし）
Drawing ドローイング（引き分け）
Full Draw フルドロー （会）
Release リリース（離れ）
Follow Through フォロースルー（残身(残心とも)）
*/

public enum ArcheryState
{
    NONE = 0,           
    STANCE,
    SET,
    NOCKING,
    SET_UP,
    DRAWING,
    FULL_DRAW,
    RELEASE,
    FOLLOW_THROUGH,
    NUM,
}

public class VRArcheryController3 : MonoBehaviour
{

    public ArcheryState archeryState;

    [SerializeField]
    Bow2 bow;

    [SerializeField]
    float shotPower;

    [SerializeField]
    SteamVR_TrackedObject rController;

    [SerializeField]
    SteamVR_TrackedObject lController;

    [SerializeField]
    TrackingTransform arrowCamera;

    [SerializeField]
    float powerMagnitude;

    [SerializeField]
    float powMagMax;

    [SerializeField]
    float vibration;

    public GameObject drawingStandard;

    public bool hasArrow;

    public Vector3 basePos = Vector3.zero;

    [SerializeField]
    PredictionLine preLine;

    [SerializeField]
    float drawLimit;

    private void Start()
    {
        drawingStandard = new GameObject("ArrowStandard");
        drawingStandard.transform.SetParent(bow.transform);
        preLine.CreateLine();

        bow.StringCenter.position = bow.StringBasePos.position;

        var am = AudioManager.Instance;
    }

    void Update()
    {
        var lDevice = ViveController.Instance.GetLeftDevice();
        var rDevice = ViveController.Instance.GetRightDevice();

        var rTransform = ViveController.Instance.RightController.transform;

        //矢を生成して、弓にセットする
        if (lDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) &&
            hasArrow == false)
        {
            bow.CreateArrow();
            var pos = bow.arrow.Tail.transform.position;
            bow.StringCenter.position = pos;

            hasArrow = true;

            //追尾カメラをセット
            arrowCamera.target = bow.arrow.transform;
            arrowCamera.onUsed = true;

            //予測線のアルファ値の初期化
            var col = preLine.lineRend.material.color;
            col.a = 1.0f;
        }

        if (rDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            var pos = rDevice.transform.pos;
            basePos = pos;
            drawingStandard.transform.position = pos;
            UpdateLine();
        }
        else if (rDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger) &&
            hasArrow == true)
        {
            //弓の弦
            var cenPos = bow.arrow.Tail.transform.position;
            bow.StringCenter.position = cenPos;

            //弓の引き具合の計算　引いてるほどパワーが強くなる
            var pos = rDevice.transform.pos;
            var curMov = pos - drawingStandard.transform.position;
            var mag = curMov.magnitude;

            var back = -bow.transform.forward;
            var dist = Vector3.Dot(back, curMov);

            //if (dist > drawLimit) dist = drawLimit;
            dist = Mathf.Clamp(dist, 0.0f, drawLimit);

            //弓の弦に合った位置に矢を移動させる
            bow.StringCenter.position = bow.StringBasePos.position + back * dist;
            bow.arrow.SetPosFromTail(bow.StringCenter.position);

            bow.SetPower(dist * powerMagnitude);

            //振動
            rDevice.TriggerHapticPulse((ushort)(dist * vibration));

            UpdateLine();
            preLine.lineRend.material.color = new Color(1f, 0f, 0f, 1f);

            Debug.Log("dist: " + dist);

        }
        else if (rDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) &&
            hasArrow == true)
        {
            bow.Shoot();
            hasArrow = false;
            arrowCamera.target = bow.arrow.transform;

            bow.StringCenter.position = bow.StringBasePos.position;

            //予測線をフェードアウト
            StartCoroutine(Utility.TimeCrou(1.0f, 
                (f) => 
                {
                    var col = preLine.lineRend.material.color;
                    preLine.lineRend.material.color = new Color(col.r, col.g, col.b, 1.0f - f);
                }));

            Debug.Log("ShotPower " + bow.curPower);

        }

        if (hasArrow)
        {
           
        }

    }


    void UpdateLine()
    {
        var data = new CalculatedData();
        data.dir = bow.transform.forward;
        data.speed = bow.curPower;
        data.startPos = bow.transform.position;
        data.grav = bow.arrowGrav;
        preLine.CalcLine(data);
    }

}
