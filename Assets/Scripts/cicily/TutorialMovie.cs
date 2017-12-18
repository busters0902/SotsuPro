using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class TutorialMovie : MonoBehaviour {

    
    [SerializeField]
    private VideoPlayer[] vScreen;
    //[SerializeField]
    public VideoClip[] vPlayers;

    public string videoName;

    [SerializeField]
    AudioSource audioSource;

    int videoNum = 9;

    int nowIndex = 0;

    string btName = "Button0";

    // Use this for initialization
    void Start () {
        //vPlayer.clip = Resources.Load<VideoClip>("");
        //vPlayers.url = "Assets//Resources//TutorialMovie//01.mp4";
        
        vPlayers = new VideoClip[videoNum];
        for (int i= 0; i < videoNum; i++)
        {
            // vPlayers[i].url = "Assets//Resources//TutorialMovie//0" + (i + 1) + ".mp4";
            // vPlayers[i] = this.gameObject.AddComponent<VideoPlayer>();
            //vPlayers[i].url = "Assets//Resources//TutorialMovie//01.mp4";
            vPlayers[i] = Resources.Load("TutorialMovie/0"+(i+1)) as VideoClip;
            vScreen[i].gameObject.transform.localPosition = new Vector3(0.0f, -1.0f, -0.508f);
            vScreen[i].clip = vPlayers[i];
            vScreen[i].audioOutputMode = VideoAudioOutputMode.AudioSource;
            vScreen[i].EnableAudioTrack(0, true);
            vScreen[i].SetTargetAudioSource(0, audioSource);
        }
        vScreen[0].gameObject.transform.localPosition = new Vector3(0.0f, 0.146f, -0.508f);
        vScreen[0].Play();
    }
	
	// Update is called once per frame
	void Update () {


        rayCastMouse();
        
	}

    public float distance = 100f;//レイが届く距離

    void rayCastMouse()
    {
        if (ViveController.Instance.ViveRight)
        {
            var trans = ViveController.Instance.RightController.transform;
            // クリックしたスクリーン座標をrayに変換
            Ray ray = new Ray(trans.position, trans.forward);
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
        Debug.Log(name);
        vScreen[nowIndex].gameObject.transform.localPosition = new Vector3(0.0f, -1.0f, -0.508f);
        vScreen[nowIndex].Pause();
        vScreen[nowIndex].time = 0;
        if (name.Length < 7) return;
        var num_str = name.Substring(7);
        
      
         if (name.Contains(btName + num_str))
            {
            var num = int.Parse(num_str);
            nowIndex = num;
            vScreen[num].clip = vPlayers[num];
            vScreen[num].gameObject.transform.localPosition = new Vector3(0.0f, 0.146f, -0.508f);
            vScreen[num].Play();
        }
       
       
        

    }


}
