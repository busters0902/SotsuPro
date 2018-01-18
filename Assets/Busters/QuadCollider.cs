using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//GameObject Quadに対応したコライダー
public class QuadCollider : MonoBehaviour
{

    public Quadangle col;

    public float rad;

	void Start ()
    {
        col = new Quadangle();

        var size = transform.lossyScale;
        var pos = transform.position;
        Debug.Log("コライダーのサイズ :" + size );

        var u = transform.right * size.x / 2f;
        var v = transform.up * size.y / 2f;

        rad = size.x / 2f;

        //左上　右上　右下　左下
        col.p[0] = transform.position - u + v;
        col.p[1] = transform.position + u + v;
        col.p[2] = transform.position + u - v;
        col.p[3] = transform.position - u - v;


        //DebugLogPositions();
    }

    public void UpdateCollider()
    {
        var size = transform.lossyScale;
        var pos = transform.position;
        Debug.Log("コライダーのサイズ :" + size);

        var u = transform.right * size.x / 2f;
        var v = transform.up * size.y / 2f;

        //左上　右上　右下　左下
        col.p[0] = transform.position - u + v;
        col.p[1] = transform.position + u + v;
        col.p[2] = transform.position + u - v;
        col.p[3] = transform.position - u - v;
    }

    public void DebugLogPositions()
    {
        foreach(var p in col.p)
        {
            Debug.Log(p);
        }
    }

}
