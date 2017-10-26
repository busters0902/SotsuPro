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

    public SteamVR_Controller.Device GetRightDevice(){return SteamVR_Controller.Input((int)RightController.index); }
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

    bool viveRightDown;
    bool viveRightUp;
    bool viveRight;
    bool viveLeftDown;
    bool viveLeftUp;
    bool viveLeft;

    Vector2 rightAxis;

    //void UseTest()
    //{
    //	var device = ViveController.Instance.GetRightDevice();
    //	if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    //	{

    //	}
    //}

    private void Update()
    {
        var device = ViveController.Instance.GetRightDevice();
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            viveRightDown = true;
        else
            viveRightDown = false;

        Debug.Log(" " + device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x);

        Vector2 position = device.GetAxis();
        Debug.Log("x: " + position.x + " y: " + position.y);

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            viveRightUp = true;
        else
            viveRightUp = false;
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            viveRight = true;
        else
            viveRight = false;

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

        float valueX = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
        float valueY = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).y;
        rightAxis = new Vector2(valueX, valueY);

        //Debug.Log("VRコン: " + rightAxis);
    }


    //

}

public static class VRUtility
{
	public static void CreateVRController()
	{
		var obj = (GameObject)MonoBehaviour.Instantiate(Resources.Load("Prefabs/VRController"));
		MonoBehaviour.DontDestroyOnLoad(obj);
	}
}



