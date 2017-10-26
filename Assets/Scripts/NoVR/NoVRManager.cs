using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoVRManager : MonoBehaviour
{

	[SerializeField]
	HandGun handGun;

	[SerializeField]
	float shotPower;

	Vector2 curMousePos;
	Vector2 prevMousePos;

	void Update ()
	{
		
		if(Input.GetMouseButtonDown(0))
		{
			handGun.ToAttack(shotPower);
		}

		if (Input.GetMouseButtonDown(1))
		{
			curMousePos = Input.mousePosition;
			prevMousePos = Input.mousePosition;
		}
		else if (Input.GetMouseButton(1))
		{
			curMousePos = Input.mousePosition;

			var diff = prevMousePos - curMousePos;

			//handGun.transform.rotation *= Quaternion.FromToRotation(curMousePos, prevMousePos);
			var rotY = Quaternion.AngleAxis(-diff.x, Vector3.up);
			var rotX = Quaternion.AngleAxis(-diff.y, Vector3.right);

			handGun.transform.rotation *= rotX * rotY;
			//Debug.Log( curMousePos + " : " + prevMousePos);
			prevMousePos = curMousePos;
		}


	}
}
