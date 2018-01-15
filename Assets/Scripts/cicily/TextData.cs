using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextData : MonoBehaviour {

    [SerializeField]
    Canvas canv;

    [SerializeField]
    GameObject prehub;

	// Use this for initialization
	void Start (){

        TextManager.Instance.SetCanvas(canv);
        TextManager.Instance.SetPrefab(prehub);
        TextManager.Instance.addTextFrash(new Vector3(14.0f, 5.0f, 10.0f), new Vector3(0.1f, 0.1f, 0.0f),15, "加藤純二", "うんこちゃん", false);

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //TextManager.Instance.addText("加藤純一","うんこちゃん");
            TextManager.Instance.addTextFrash(new Vector3(14.0f,5.0f,10.0f), new Vector3(0.1f, 0.1f, 0.0f),15,"加藤純二", "ちんこちゃん",true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //TextManager.Instance.addText("加藤純一","うんこちゃん");
            var t = TextManager.Instance.texts["加藤純二"];
            t.GetComponent<Text>().text = "ちんこちゃん";
            t.GetComponent<TextFrash>().setAlpha(0.5f);
            t.GetComponent<TextFrash>().setColor(1.0f,0.0f,0.0f);
        }



    }
}
