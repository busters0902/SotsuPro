using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRArcheryController : MonoBehaviour
{
    [SerializeField]
    Bow bow;

    [SerializeField]
    SteamVR_TrackedObject rController;

    [SerializeField]
    SteamVR_TrackedObject lController;

    [SerializeField]
    TrackingTransform arrowCamera;

    void Update()
    {
        var lDevice = ViveController.Instance.GetLeftDevice();
        var rDevice = ViveController.Instance.GetRightDevice();


        if ( lDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            bow.CreateArrow();
        }
        else if (lDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            bow.UpdateDraw(0.01f);
        }
        else if (lDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            bow.Shoot();
            arrowCamera.target = bow.arrow.transform;
        }

    }
}
