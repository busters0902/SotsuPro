using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{

	[SerializeField]
	SteamVR_TrackedObject rController;

	[SerializeField]
	SteamVR_TrackedObject lController;

	float rayLength;

	bool catchesObject;
	GameObject catchingObject;
	Rigidbody catchObjRig;


	[SerializeField]
	float throwPower;

	Vector3 eCurPos;
	Vector3 ePrevPos;


	//文字書くよう
	[SerializeField]
	GameObject linePrefab;
	[SerializeField]
	LineRenderer tgtline;
	List<Vector3> linePoss;
	bool drawsLine = false;

	void Awake()
	{
		VRUtility.CreateVRController();
	}

	void Start()
	{
        Debug.Log(gameObject + " Start");
		//出来ればAwakeで
		ViveController.Instance.SetRightController(rController);
		ViveController.Instance.SetLeftController(lController);

	}

	void Update()
	{
		RaycastHit hit;
		Ray ray = new Ray(rController.transform.position, rController.transform.forward);

		var rDevice = ViveController.Instance.GetRightDevice();

		if (Physics.Raycast(ray, out hit))
		{
			Transform objectHit = hit.transform;


			if (objectHit.tag == "DrawObj" &&
				rDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				var obj = Instantiate<GameObject>(linePrefab);
				var line = obj.GetComponent<LineRenderer>();
				tgtline = line;
                line.material = new Material(Shader.Find("Mobile/Particles/Additive"));
                linePoss = new List<Vector3>();
				linePoss.Add(hit.point);
				//line.SetPositions();
				drawsLine = true;
				Debug.Log("書き込み開始");
			}
			else if ( drawsLine == true &&
			rDevice.GetPress(SteamVR_Controller.ButtonMask.Trigger))
			{
				linePoss.Add(hit.point);

				var array = linePoss.ToArray();
				tgtline.positionCount = array.Length;
				tgtline.SetPositions(array);
				//Debug.Log("書き込み中" + linePoss.Count + " : " + hit.point);
			}


			//if (objectHit.tag == "Prehensile" && Input.GetMouseButtonDown(0))    //ここ
			if (objectHit.tag == "Prehensile" &&
				rDevice.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
			{
				Debug.Log("Hit Prehensile Object");
				rayLength = (objectHit.position - ray.origin).magnitude;

				catchesObject = true;
				catchingObject = objectHit.gameObject;
				catchObjRig = catchingObject.GetComponent<Rigidbody>();
				catchObjRig.useGravity = false;
				catchObjRig.velocity = Vector3.zero;

				eCurPos = objectHit.position;
				ePrevPos = objectHit.position;
			}

		}

		if(rDevice.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
		{
			drawsLine = false;
		}

		//if (Input.GetMouseButtonUp(0) && catchesObject) //ここ
		if (rDevice.GetPressUp(SteamVR_Controller.ButtonMask.Trigger)
			&& catchesObject) //ここ
		{
			catchesObject = false;
			catchingObject = null;
			catchObjRig.useGravity = true;

			var vec = eCurPos - ePrevPos;
			catchObjRig.velocity = vec * throwPower;
		}

		if (catchesObject && catchingObject != null)
		{
			//catchingObject.transform.position = ray.GetPoint(rayLength);

			catchObjRig.angularVelocity = Vector3.zero;

			catchObjRig.MovePosition(ray.GetPoint(rayLength));

			ePrevPos = eCurPos;
			eCurPos = catchObjRig.transform.position;

            Debug.Log("投げるやつ"+ ePrevPos + " : " + eCurPos );
		}

	}
}
