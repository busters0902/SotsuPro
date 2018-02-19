using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleLength : MonoBehaviour
{
    [SerializeField]
    GameObject left;

    [SerializeField]
    GameObject right;

    public bool InAngle(GameObject obj)
    {

        var tgtDir1 = left.transform.position - obj.transform.position;
        var crs1 = Vector3.Cross(obj.transform.forward, tgtDir1);

        //左に
        if (crs1.y <= 0)
        {
            return false;
        }

        var tgtDir2 = right.transform.position - obj.transform.position;
        var crs2 = Vector3.Cross(obj.transform.forward, tgtDir2);

        //右に
        if (crs2.y >= 0)
        {
            return false;
        }

        return true;
    }

}