using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

	[SerializeField]
	HandGun handGun;

	[SerializeField]
	float shotPower;

	public bool canUseGun = true;

    [SerializeField]
    bool isRight;

	void Update ()
	{
        var device = GetDev(isRight);

		if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && canUseGun)
		{ 
			handGun.ToAttack(shotPower);
		}
	}

    SteamVR_Controller.Device GetDev(bool isRight)
    {
        if (isRight) return ViveController.Instance.GetRightDevice();
        else return ViveController.Instance.GetLeftDevice();
    }
}
