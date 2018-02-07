using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour
{

	public static ViveController Instance { get; private set; }

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(this);
			return;
		}

		Debug.Log("Awake : " + gameObject.ToString());
	}

	public void DontDestroy()
	{
		DontDestroyOnLoad(this.gameObject);
	}


    
    public SteamVR_TrackedObject RightController;
    public SteamVR_TrackedObject LeftController;

    public SteamVR_Controller.Device GetRightDevice(){ return SteamVR_Controller.Input((int)RightController.index); }
	public SteamVR_Controller.Device GetLeftDevice() { return SteamVR_Controller.Input((int)LeftController.index); }

	public void SetRightController(SteamVR_TrackedObject obj)
	{
		RightController = obj;
        Debug.Log("右コントローラーの接続");
    }

	public void SetLeftController(SteamVR_TrackedObject obj)
	{
		LeftController = obj;
        Debug.Log("左コントローラーの接続");
	}

    //コルーチン内では通常のデバイスからの判定を行えないので
    bool viveRightDown;
    public bool ViveRightDown{ get { return viveRightDown; } }
    bool viveRightUp;
    public bool ViveRightUp { get { return viveRightUp; } }
    bool viveRight;
    public bool ViveRight { get { return viveRight; } }
    bool viveLeftDown;
    public bool ViveLeftDown { get { return viveLeftDown; } }
    bool viveLeftUp;
    public bool ViveLeftUp { get { return viveLeftUp; } }
    bool viveLeft;
    public bool ViveLeft { get { return viveLeft; } }

    Vector2 rightAxis;
    public Vector2 RightAxis { get { return rightAxis; } }

    Vector2 leftAxis;
    public Vector2 LeftAxis { get { return leftAxis; } }

    private void Update()
    {

        //右
        var device = ViveController.Instance.GetRightDevice();
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            viveRightDown = true;
        else
            viveRightDown = false;

        //Debug.Log(" " + device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x);

        Vector2 position = device.GetAxis();
        //Debug.Log("x: " + position.x + " y: " + position.y);

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            viveRightUp = true;
        else
            viveRightUp = false;
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            viveRight = true;
        else
            viveRight = false;

        //左
        var device2 = ViveController.Instance.GetLeftDevice();
        if (device2.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            viveLeftDown = true;
        else
            viveLeftDown = false;

        if (device2.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            viveLeftUp = true;
        else
            viveLeftUp = false;
        if (device2.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            viveLeft = true;
        else
            viveLeft = false;

        float valueRX = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
        float valueRY = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).y;
        rightAxis = new Vector2(valueRX, valueRY);

        //Debug.Log("VRコン: " + rightAxis);

        float valueLX = device2.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
        float valueLY = device2.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).y;
        rightAxis = new Vector2(valueLX, valueLY);

    }

}

public static class VRUtility
{
	public static void CreateVRController()
	{
		var obj = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/VRController"));
		MonoBehaviour.DontDestroyOnLoad(obj);
	}
}



