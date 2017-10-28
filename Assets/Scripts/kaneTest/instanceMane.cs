using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instanceMane : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public GameObject ya; 
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(ya);
        }
	}
}
