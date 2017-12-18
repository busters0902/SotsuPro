using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class TutorialMovie : MonoBehaviour {

    [SerializeField]
    private VideoPlayer vPlayer;

    

    public string videoName;

    public bool isPlay = false;

    // Use this for initialization
    void Start () {
        //vPlayer.clip = Resources.Load<VideoClip>("");
        vPlayer.url = "Assets//Resources//TutorialMovie//01.mp4";
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log(videoName);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    // vPlayer.url = videoName;
        //    isPlay = !isPlay;
        //    if (isPlay == true)
        //    {
        //        OnPlay();
        //    }
        //    if (isPlay == false)
        //    {
        //        OnPause();
        //    }
        //}
        rayCastMouse();
        
	}

    void OnPlay()
    {
        vPlayer.Play();
    }

    void OnPause()
    {
        vPlayer.Pause();
    }

    public float distance = 100f;//レイが届く距離

    void rayCastMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // クリックしたスクリーン座標をrayに変換
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Rayの当たったオブジェクトの情報を格納する
            RaycastHit hit = new RaycastHit();
           
            if (Physics.Raycast(ray, out hit, distance))
            {
                // rayが当たったオブジェクトの名前を取得
                var objectName = hit.collider.gameObject.name;
                loadMovie(objectName);
            }
        }
    }

    void loadMovie(string name)
    {
        if (name == "Button01") {
            vPlayer.url = "Assets//Resources//TutorialMovie//01.mp4";
        }
        if (name == "Button02")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//02.mp4";
        }
        if (name == "Button03")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//03.mp4";
        }
        if (name == "Button04")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//04.mp4";
        }
        if (name == "Button05")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//05.mp4";
        }
        if (name == "Button06")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//06.mp4";
        }
        if (name == "Button07")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//07.mp4";
        }
        if (name == "Button08")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//08.mp4";
        }
        if (name == "Button09")
        {
            vPlayer.url = "Assets//Resources//TutorialMovie//09.mp4";
        }
    }
}
