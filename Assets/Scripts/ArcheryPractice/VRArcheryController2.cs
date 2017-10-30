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

    public GameObject drawingStandard;


    public bool hasArrow;

    public Vector3 basePos = Vector3.zero;

    private void Start()
    {
        drawingStandard = new GameObject("ArrowStandard");
        drawingStandard.transform.SetParent(bow.transform);
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
            arrowCamera.target = bow.arrow.transform;
        }

        if (rDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //bow.UpdateDraw(0.01f);
            var pos = rDevice.transform.pos;
            basePos = pos;
            drawingStandard.transform.position = pos;
            //Debug.Log("lDevice Pos: "+ pos);
        }
        else if (rDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            var pos = rDevice.transform.pos;
            var curMov = drawingStandard.transform.position - pos;
            var mag = curMov.magnitude;
            if (mag > powMagMax) mag = powMagMax;
            //Debug.Log("lDevice Pos: " + curPos);
            Debug.Log(drawingStandard.transform.position +" "+ pos + " " +  curMov);
            float z = transform.position.z;
            bow.arrow.SetPosFromTail(bow.transform.right * 0.01f + bow.transform.position + -bow.transform.forward * mag);
            bow.curPower = mag * powerMagnitude;

            //振動
        }
        else if (rDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) &&
            hasArrow == true)
        {
            hasArrow = false;
            //bow.curPower = shotPower;
            bow.Shoot();
            arrowCamera.target = bow.arrow.transform;

            Debug.Log("ShotPower "+ bow.curPower);
            //振動
        } 

        //パワー
        //振動



    }
}
