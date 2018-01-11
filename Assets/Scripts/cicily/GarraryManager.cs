using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraryManager : MonoBehaviour {

    //[SerializeField]
    public GameObject[] garrays;

    public bool isJump;

    public bool isEnd;

    public float puchTime;

    // Use this for initialization
    void Start () {

        isJump = true;

        isEnd = false;

        puchTime = 1.0f;

        StartCoroutine(jump());

	}

   
	
	// Update is called once per frame
	void Update () {
              
    }

    int rand ;

    IEnumerator jump()
    {

        while (!isEnd)
        {

           rand = Random.Range(0, 27);

            iTween.PunchPosition(garrays[rand], iTween.Hash("y", 0.1,"time", puchTime));

            yield return new WaitForSeconds(0.1f);          

        }

    }

   
}
