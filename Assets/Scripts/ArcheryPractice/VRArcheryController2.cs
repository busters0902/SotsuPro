using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRArcheryController2 : MonoBehaviour
{
    [SerializeField]
    Bow bow;

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
            hasArrow = true;
            //追尾カメラをセット
            arrowCamera.target = bow.arrow.transform;
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
            //弓の引き具合の計算　引いてるほどパワーが強くなる
            var pos = rDevice.transform.pos;
            var curMov = drawingStandard.transform.position - pos;
            var mag = curMov.magnitude;
            //if (mag > powMagMax) mag = powMagMax;
            //Debug.Log(drawingStandard.transform.position +" "+ pos + " " +  curMov);

            //bow.arrow.SetPosFromTail(bow.transform.right * 0.01f + bow.transform.position + -bow.transform.forward * mag);
            bow.curPower = mag * powerMagnitude;
            //振動
            rDevice.TriggerHapticPulse((ushort)(mag * vibration));
            //lDevice.TriggerHapticPulse((ushort)(mag * vibration));

            UpdateLine();

            Debug.Log("mag: " + mag);
            

        }
        else if (rDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) &&
            hasArrow == true)
        {
            bow.Shoot();
            hasArrow = false;
            arrowCamera.target = bow.arrow.transform;

            Debug.Log("ShotPower "+ bow.curPower);
      
        } 

        if(hasArrow)
        {
            //振動
            //lDevice.TriggerHapticPulse(1000);
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
