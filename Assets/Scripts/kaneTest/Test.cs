using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //FadeControl.Instance.FadeStart();
	}
    [SerializeField]
    Transform target;

	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, -Vector3.up, out hit))
            {
                Debug.Log("Found an object - distance: ");

                Debug.Log("Found an object - distance: " + hit.collider.gameObject.name);
            }
        }
        

    }
}
