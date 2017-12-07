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

        while (true)
        {

            timer.Update();
            //Debug.Log("timer: " + timer.time);

            if (timer.time >= limTime) break;

            var t = timer.time / limTime;

            act(t);

            yield return null;
        }
        act(limTime);

    }

<<<<<<< HEAD
    public static IEnumerator TimerCrou(float limTime, System.Action act)
    {
        Debug.Log("Start TimerCrou");

        yield return new WaitForSeconds(limTime);

        act();
    }

    public static IEnumerator Vibration( float spd, float rate, System.Action<float> act)
=======
    public static IEnumerator Vibration(float spd, float rate, System.Action<float> act)
>>>>>>> eb83549e861e57f497a8c0f70fc7c04d84d4810c
    {

        Debug.Log("Start TimeCrou");
        var timer = new Timer();
        timer.Initialize();

        while (true)
        {
            timer.Update();
            float b = rate * (spd - timer.time);
            float timeRate = timer.time * spd;
            var a = Mathf.Cos(timeRate) * b;

            act(a);

            if (timeRate > 0.01f) break;

            yield return null;
        }

        Debug.Log("終了");

    }

    public static float CosWave(float timeRate, float width)
    {
        return Mathf.Cos(timeRate) * width;
    }



}
