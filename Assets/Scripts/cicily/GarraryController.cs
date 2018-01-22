using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraryController : MonoBehaviour {

    public Garrary[] garrarys;

	// Use this for initialization
	void Start () {
        //standardJump();
        //highJump();
        Debug.Log(garrarys.Length);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void highJump()
    {
        for (int i =0; i < garrarys.Length; i++)
        {
            garrarys[i].highJump = true;
        }
    }

    public void startJump()
    {
        for (int i = 0; i < garrarys.Length; i++)
        {
            garrarys[i].isJump = true;
        }
    }

    public void stopJump()
    {
        for (int i = 0; i < garrarys.Length; i++)
        {
            garrarys[i].isJump = false;
            garrarys[i].highJump = false;
        }
    }

    
}
