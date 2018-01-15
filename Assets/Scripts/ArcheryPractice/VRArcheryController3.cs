using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//VRシーンでの弓のコントローラー

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

    public Bow2 bow;

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

    [SerializeField, Range(0, 1), Tooltip("弓の弦を引ける距離")]
    float drawingDist;

    public GameObject drawingStandard;

    public bool hasArrow;
    private bool isDrawing;

    public Vector3 basePos = Vector3.zero;

    [SerializeField]
    PredictionLine preLine;

    [SerializeField]
    float drawLimit;

    //updateで処理するか
    public bool useBow;

    public bool isReverse;

    [SerializeField]
    GameObject bowObject;

    [SerializeField]
    GameObject leftHand;

    [SerializeField]
    GameObject rightHand;

    [SerializeField]
    burekeigen shakeMitig;

    //引き切ったかどうか
    bool isMaxDrawing;
    bool isMaxDrawingFirst;

    bool isPlayDrawingSE;
    float minPlayDrawingSE;

    //動作ごとのコールバック
    public System.Action setArrowCall = null;   //矢をセットした時
    public System.Action fullDrawingCall = null;    //弦を引き切った時
    private System.Action shotedCall = null;    //撃ったとき
    public System.Action ShotedCall { set { shotedCall = value; } }

    private List<Arrow3> arrows;

    void ResetReverseMode()
    {
        bowObject.transform.SetParent(null);

        if(isReverse)
        {
            leftHand.transform.GetChild(0).gameObject.SetActive(true);
            rightHand.transform.GetChild(0).gameObject.SetActive(false);
            bowObject.transform.SetParent(rightHand.transform);
        }
        else
        {
            leftHand.transform.GetChild(0).gameObject.SetActive(false);
            rightHand.transform.GetChild(0).gameObject.SetActive(true);
            bowObject.transform.SetParent(leftHand.transform);
        }

    }



    private void Start()
    {
        drawingStandard = new GameObject("ArrowStandard");
        drawingStandard.transform.SetParent(bow.transform);
        preLine.CreateLine();

        isMaxDrawing       = false ;
        isMaxDrawingFirst  = false ;
        shakeMitig.enabled = false ;

        bow.StringCenter.position = bow.StringBasePos.position;

        arrows = new List<Arrow3>();

        ResetReverseMode();
        
    }

    void Update()
    {
        if(useBow) UseBow();
    }

    public void UseBow()
    {

        SteamVR_Controller.Device bowDevice = null;
        SteamVR_Controller.Device hundleDevice = null;
        //デバイスの左右持ちで反転
        if (isReverse)
        {
            bowDevice = ViveController.Instance.GetRightDevice();
            hundleDevice = ViveController.Instance.GetLeftDevice();
        }
        else
        {
            bowDevice = ViveController.Instance.GetLeftDevice();
            hundleDevice = ViveController.Instance.GetRightDevice();
        }

        var rTransform = ViveController.Instance.RightController.transform;

        

        //弓を引く
        if ( hundleDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)
            && IsAreaDrawingString(hundleDevice.transform.pos, drawingDist)
            && isDrawing == false && hasArrow == true)
        {
            var pos = hundleDevice.transform.pos;
            basePos = pos;
            drawingStandard.transform.position = pos;
            isDrawing = true;
            UpdateLine();

            //※弓を引く音
            AudioManager.Instance.PlaySE("弦引き");
            Debug.Log("Played SE: 弦引き");

        }
        //弓を引いている
        else if ( hundleDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger)
            && isDrawing == true && hasArrow == true)
        {
            Debug.Log(bow);
            Debug.Log(bow.arrow);
            Debug.Log(bow.arrow.Tail);

            //弓の弦
            var cenPos = bow.arrow.Tail.transform.position;
            bow.StringCenter.position = cenPos;

            //弓の引き具合の計算　引いてるほどパワーが強くなる
            var pos = hundleDevice.transform.pos;
            var curMov = pos - drawingStandard.transform.position;
            var mag = curMov.magnitude;

            var back = -bow.transform.forward;
            var dist = Vector3.Dot(back, curMov);

            //if (dist > drawLimit) dist = drawLimit;
            dist = Mathf.Clamp(dist, 0.0f, drawLimit);

            //手ブレ補正を使う
            if (isMaxDrawingFirst == false)
            {
                if (dist == drawLimit)
                {
                    isMaxDrawing = true;
                    isMaxDrawingFirst = true;
                    Debug.Log("Use 手ぶれ補正");
                    shakeMitig.enabled = true;
                    //コールバック
                    fullDrawingCall();
                }
            }


            //弓の弦に合った位置に矢を移動させる
            bow.StringCenter.position = bow.StringBasePos.position + back * dist;
            bow.arrow.SetPosFromTail(bow.StringCenter.position);

            bow.SetPower(dist * powerMagnitude);

            //振動
            hundleDevice.TriggerHapticPulse((ushort)(dist * vibration));

            //※弓を引く音 クリップの延長??
            //AudioManager.Instance.PlaySE();

            UpdateLine();
            preLine.lineRend.material.color = new Color(1f, 0f, 0f, 1f);

            //Debug.Log("dist: " + dist);

        }
        //そげきっ！？
        else if ( hundleDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)
            && isDrawing == true)
        {
            bow.Shoot();
            hasArrow = false;
            isDrawing = false;
            arrowCamera.target = bow.arrow.transform;

            bow.StringCenter.position = bow.StringBasePos.position;

            //予測線をフェードアウト
            StartCoroutine(Utility.TimeCrou(1.0f,
                (f) =>
                {
                    var col = preLine.lineRend.material.color;
                    preLine.lineRend.material.color = new Color(col.r, col.g, col.b, 1.0f - f);
                }));

            //手ブレ補正を切る
            isMaxDrawing = false;
            isMaxDrawingFirst = false;
            shakeMitig.enabled = false;
            shakeMitig.ResetRotation();

            bow.arrow.useCalcIntersect = true;

            //コールバック
            shotedCall();

            //矢を射る音
            AudioManager.Instance.PlaySE("矢の飛来");
            Debug.Log("Played SE: 矢の飛来");

        }

        //矢を生成して、弓にセットする(両デバイスでトリガーを押す) 
        //※処理フレームをずらすため 引く処理より後
        if (hundleDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) &&
            hasArrow == false)
        {

            var ar = bow.CreateArrow();
            var pos = bow.arrow.Tail.transform.position;
            bow.StringCenter.position = pos;

            hasArrow = true;

            //追尾カメラをセット
            arrowCamera.target = bow.arrow.transform;
            arrowCamera.onUsed = true;

            //予測線のアルファ値の初期化
            var col = preLine.lineRend.material.color;
            col.a = 1.0f;

            arrows.Add(ar);

            setArrowCall();

        }

    }

    //予測線の更新
    void UpdateLine()
    {
        var data = new CalculatedData();
        data.dir = bow.transform.forward;
        data.speed = bow.curPower;
        data.startPos = bow.transform.position;
        data.grav = bow.arrowGrav;
        preLine.CalcLine(data);
    }

    //フィールド上の矢の破棄
    public void ClearArrows()
    {
        foreach( var obj in arrows)
        {
            Destroy(obj.gameObject);
        }
        arrows.Clear();
    }

    //弓の弦をつかめる距離
    bool IsAreaDrawingString(Vector3 pos, float dist)
    {
        
        var v = bow.StringBasePos.position - pos;

        if (v.magnitude < dist)
        {
            //Debug.Log("弦を引けます");
            return true;
        }

        //Debug.Log("弦を引く距離が遠すぎます: " + v.magnitude );
        return false;
        
    }

    public Arrow3 GetArrow()
    {
        return bow.arrow;
    }

}