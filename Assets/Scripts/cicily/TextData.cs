using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData : MonoBehaviour {



	// Use this for initialization
	void Start (){
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //TextManager.Instance.addText("加藤純一","うんこちゃん");
            TextManager.Instance.addText(new Vector3(14.0f,5.0f,10.0f), new Vector3(0.1f, 0.1f, 0.0f),"加藤純一", "うんこちゃん",true);
        }
	}
}
