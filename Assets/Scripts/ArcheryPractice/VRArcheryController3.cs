using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRArcheryController3 : MonoBehaviour
{
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

    private void Start()
    {
        drawingStandard = new GameObject("ArrowStandard");
        drawingStandard.transform.SetParent(bow.transform);
        preLine.CreateLine();
    }

    void Update()
    {
        var lDevice = ViveController.Instance.GetLeftDevice();
        var rDevice = ViveController.Instance.GetRightDevice();

        var rTransform = ViveController.Instance.RightController.transform;

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
        }

        if (rDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            var pos = rDevice.transform.pos;
            basePos = pos;
            drawingStandard.transform.position = pos;
            UpdateLine();
        }
        else if (rDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            //弓の弦
            var cenPos = bow.arrow.Tail.transform.position;
            bow.StringCenter.position = cenPos;

            //弓の引き具合の計算　引いてるほどパワーが強くなる
            var pos = rDevice.transform.pos;
            var curMov = drawingStandard.transform.position - pos;
            var mag = curMov.magnitude;

            var back = -bow.transform.forward;
            var dist = Vector3.Dot(back, pos);

            bow.StringCenter.position = bow.StringBasePos.position + back * dist;

            bow.SetPower(dist * powerMagnitude);
            //振動
            rDevice.TriggerHapticPulse((ushort)(dist * vibration));

            UpdateLine();

            //Debug.Log("mag: " + mag);
            Debug.Log("dist: " + dist);

        }
        else if (rDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) &&
            hasArrow == true)
        {
            bow.Shoot();
            hasArrow = false;
            arrowCamera.target = bow.arrow.transform;

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
