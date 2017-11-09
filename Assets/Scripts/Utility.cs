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
       
}
