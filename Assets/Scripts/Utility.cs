using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{

    public static IEnumerator DestoryObject(GameObject obj, System.Action<GameObject> act, float time = 0.0f)
    {
        yield return new WaitForSeconds(time);

        act(obj);

        Object.Destroy(obj);
    }

    public static IEnumerator TimeCrou(float limTime, System.Action<float> act)
    {
        Debug.Log("Start TimeCrou");
        var timer = new Timer();
        timer.Initialize();

        while(true)
        {
            
            timer.Update();
            Debug.Log("timer: " + timer.time);

            if (timer.time >= limTime) break;

            var t = timer.time / limTime;

            act(t);

            yield return null;
        }
        act(limTime);


        Debug.Log("End TimeCrou");
    }

       
}
