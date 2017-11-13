using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObj : MonoBehaviour {

    [SerializeField]
    GameObject pre;

	void Start () {
		
	}
	
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            var obj = Instantiate(pre);
            obj.GetComponent<Arrow3>().Shot(CalculatedData.Create(Vector3.forward,1));
            obj.GetComponent<Rigidbody>().isKinematic = false;
        }

	}
}
