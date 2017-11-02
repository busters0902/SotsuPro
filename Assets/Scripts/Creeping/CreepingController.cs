using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RockState
{
    NONE,
    LEFT,
    RIGHT,
    NUM,
}


public class CreepingController : MonoBehaviour
{

    GameObject leftRock;
    GameObject rightRock;

    Vector3 leftRockPostion;
    Vector3 rightRockPostion;

    [SerializeField]
    GameObject floorObj;

    RockState rock;


    void Start ()
    {
        leftRock = new GameObject("LeftRockPostion");
        rightRock = new GameObject("ReftRockPostion");
    }

	void Update ()
    {

        var lDevice = ViveController.Instance.GetLeftDevice();
        var rDevice = ViveController.Instance.GetRightDevice();

        //var mov = new Vector3(0,0,0);
        var vecDist = new Vector3(0,0,0);

        //右のデバイス
        if(rDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            rightRockPostion = rDevice.transform.pos;
            rock = RockState.RIGHT;
        }
        else if (rDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (rock == RockState.RIGHT)
            {
                vecDist = rDevice.transform.pos - rightRockPostion;
                rightRockPostion = rDevice.transform.pos;
            }
        }
        else if(rDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if( rock == RockState.RIGHT)
            {
                rock = RockState.NONE;
            }
        }

        //左のデバイス
        if (lDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            leftRockPostion = lDevice.transform.pos;
            rock = RockState.LEFT;
        }
        else if (lDevice.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (rock == RockState.LEFT)
            {
                vecDist = lDevice.transform.pos - leftRockPostion;
                leftRockPostion = lDevice.transform.pos;
            }
        }
        else if (lDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (rock == RockState.LEFT)
            {
                rock = RockState.NONE;
            }
        }

        //地面を移動させる
        if(rock != RockState.NONE)
        {
            vecDist.y = 0f;
            floorObj.transform.position += vecDist;
        }

    }
}
