using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garrary : MonoBehaviour
{

    //[SerializeField]
    public GameObject[] garrays;

    public bool isJump;

    public bool isEnd;

    public bool highJump;

    public float puchTime;

    public float jumpPower;

    public float waitTime;

    // Use this for initialization
    void Start()
    {

        //isJump = true;

        //isEnd = false;

        //highJump = false;

        puchTime = 1.0f;

        jumpPower = 0.3f;

        waitTime = 0.1f;

        StartCoroutine(jump());

    }



    // Update is called once per frame
    void Update()
    {

    }

    int rand;

    IEnumerator jump()
    {

        while (isEnd == false)
        {
            if (isJump == true)
            {

                checkJumpState();

                rand = Random.Range(0, 27);

                iTween.PunchPosition(garrays[rand], iTween.Hash("y", jumpPower, "time", puchTime));

                yield return new WaitForSeconds(waitTime);
            }
            yield return null;

        }

    }

    void checkJumpState()
    {
        if (highJump == true)
        {
            jumpPower = 1.5f;

            waitTime = 0.005f;
        }
        if (highJump == false)
        {
            jumpPower = 0.3f;

            waitTime = 0.1f;
        }

    }

}
