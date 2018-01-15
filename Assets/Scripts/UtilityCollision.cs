using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityCollision
{

    //スカラー三重積
    public static float ScalarTriple(Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Dot(a, Vector3.Cross(b,c)) ;
    }

    //線分と四辺形の交差判定
    public static bool IntersectSegmentQuadrangle( Vector3 s1, Vector3 s2,  Quadangle q, ref Vector3 cross)
    {
        Vector3 pa = q.p[0] - s1;
        Vector3 pb = q.p[1] - s1;
        Vector3 pc = q.p[2] - s1;
        Vector3 pd = q.p[3] - s1;
        Vector3 pq = s2     - s1;

        if (!IntersectSegmentPlane(s1, s2, q.p[0], q.p[1], q.p[2], ref cross))
        {
            cross = Vector3.zero;
            return false;
        }

        Vector3 crs = Vector3.Cross(pc, pq);

        float u = Vector3.Dot(pa, crs);
        if (u >= 0f)
        {
            //ACB
            float v = -Vector3.Dot(pb, crs);
            if (v < 0f) return false;
            float w = ScalarTriple(pq, pb, pa);
            if (w < 0f)
            {
                return false;
            }
            //cross = (u * pa + v * pc + w * pb) / (u + v + w) + s1;
            //cross = (u * pa + v * pc + w * pb) / (u + v + w);
        }
        else
        {
            //ADC
            float v = Vector3.Dot(pd, crs);
            if (v < 0) return false;
            float w = ScalarTriple(pq, pa, pd);
            if (w < 0) return false;
            u = -u;

            //cross = (u * pd + v * pa + w * pc) / (u + v + w);
            //cross = (u * pd + v * pa + w * pc) / (u + v + w) + s1;
        }

        return true;
    }

    //線分と円形(3D)の交差判定
    public static bool IntersectSegmentCircle(Vector3 s1, Vector3 s2, Quadangle q, float r, ref Vector3 cross)
    {
        Vector3 pa = q.p[0] - s1;
        Vector3 pb = q.p[1] - s1;
        Vector3 pc = q.p[2] - s1;
        Vector3 pd = q.p[3] - s1;
        Vector3 pq = s2 - s1;

        if (!IntersectSegmentPlane(s1, s2, q.p[0], q.p[1], q.p[2], ref cross))
        {
            cross = Vector3.zero;
            return false;
        }

        var pos = (q.p[0] + q.p[2]) / 2;
        if (!CollidePointSphere(cross, pos, r))
        {
            return false;
        }

        return true;
    }

    //線分と平面の交差判定
    public static bool IntersectSegmentPlane(Vector3 s1, Vector3 s2, Vector3 d, Vector3 e, Vector3 f , ref Vector3 q)
    {
        Vector3 pn = Vector3.Cross( e - d, f - d  );
        float pd = Vector3.Dot(pn , d);

        Vector3 s = s2 - s1;
        float t = (pd - Vector3.Dot(pn, s1)) / Vector3.Dot(pn, s);
        if(t >= 0f && t <= 1f)
        {
            q = s1 + t * s;
            return true;
        }

        return false;
    }

    //点と球の判定、(点、球の中心、球の半径)
    public static bool CollidePointSphere( Vector3 p, Vector3 c, float r )
    {
        Vector3 d = c - p;
        float dist = Vector3.Dot(d, d);

        return dist <= r*r;
    }

    
}

[System.Serializable]
public class CollisionQuad
{
    public Vector3 center;

    public Vector3 u;

    public Vector3 v;
}

/// <summary>
/// 衝突判定用の四辺形データ
/// </summary>
[System.Serializable]
public class Quadangle
{
    public Vector3[] p;

    public Quadangle()
    {
        p = new Vector3[4];
        for(int i = 0; i < p.Length; i++)
        {
            p[i] = Vector3.zero;
        }
    }
}