using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoVR2SceneController : MonoBehaviour
{

	[SerializeField]
	Camera camera;

	[SerializeField]
	GameObject linePrefab;
	[SerializeField]
	LineRenderer tgtline;
	List<Vector3> linePoss;


	float rayLength;

	bool catchesObject;
	GameObject catchingObject;
	Rigidbody catchObjRig;


	[SerializeField]
	float throwPower;

	Vector3 eCurPos;
	Vector3 ePrevPos;


	void Start ()
	{
		
	}
	
	void Update ()
	{
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay(Input.mousePosition); //ここ

		//ray.GetPoint();

		if (Physics.Raycast(ray, out hit))
		{
			Transform objectHit = hit.transform;
			

			//Debug.Log(objectHit.tag);

			if(Input.GetMouseButtonDown(0))
			{
				var obj = Instantiate<GameObject>(linePrefab);
				var line = obj.GetComponent<LineRenderer>();
				tgtline = line;
				linePoss = new List<Vector3>();
				linePoss.Add(hit.point);
				//line.SetPositions();
				Debug.Log("書き込み開始");
			}
			else if (Input.GetMouseButton(0))
			{
				linePoss.Add(hit.point);

				var array = linePoss.ToArray();
				tgtline.positionCount = array.Length;
				tgtline.SetPositions(array);
				//Debug.Log("書き込み中" + linePoss.Count + " : " + hit.point);
			}


			if (objectHit.tag == "Prehensile" && Input.GetMouseButtonDown(0))    //ここ
			{
				//Debug.Log("Hit Prehensile Object");
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

		if(Input.GetMouseButtonUp(0) && catchesObject) //ここ
		{
			catchesObject = false;
			catchingObject = null;
			catchObjRig.useGravity = true;

			var vec = eCurPos - ePrevPos;
			catchObjRig.velocity = vec * throwPower; 
		}

		if(catchesObject && catchingObject != null)
		{
			//catchingObject.transform.position = ray.GetPoint(rayLength);
			float wheel = Input.GetAxis("Mouse ScrollWheel");   //ここ

			float t = 1.0f;
			rayLength += wheel * t;

			catchObjRig.angularVelocity = Vector3.zero;

			catchObjRig.MovePosition(ray.GetPoint(rayLength));
			//catchObjRig.velocity = Vector3.zero;

			ePrevPos = eCurPos;
			eCurPos = catchObjRig.transform.position;

			

		}

	}

	//Rayの長さを固定して移動

}
 