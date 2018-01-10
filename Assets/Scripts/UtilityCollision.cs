using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityCollision
{

    //intersect segment vs quad
    public static bool IntersectSegmentQuad(Vector3 p1, Vector3 p2, CollisionQuad q)
    {
        return false;
    }

    //Vector3.Dot();
    //Vector3.Cross();

    public static float ScalarTriple(Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Dot(a, Vector3.Cross(b,c)) ;
        //return Vec3Dot(pV1, &Vec3Cross(pV2, pV3));
    }

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

        //Vector3 crs = Vector3.Cross(pc, pq);

        //float u = Vector3.Dot(crs, pq);
        //if (u >= 0)
        //{
        //    //ACB
        //    float v = -Vector3.Dot(crs, pb);
        //    if (v < 0) return false;
        //    float w = ScalarTriple(pq, pb, pa);
        //    if (w < 0)
        //    {
        //        return false;
        //    }
        //    cross = (u * pa + v * pc + w * pb) / (u + v + w) + s1;

        //}
        //else
        //{
        //    cross = Vector3.zero;
        //    return false;
        //    ////ADC
        //    //float v = Vector3.Dot(crs, pd);
        //    //if (v < 0) return false;
        //    //float w = ScalarTriple(pq, pa, pd);
        //    //if (w < 0) return false;
        //    //u = -u;
        //    ////衝突位置は体積比による重心位置になる		
        //    //cross = (u * pd + v * pa + w * pc) / (u + v + w) + s1;
        //}



        return true;
    }

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


    //bool Collision::IntersectSegmentQuadrangle( const Segment* pS, const Quadrangle* pQ, Vec3f* pC)
    //{

    //	  Vec3f pa = pQ->p[0] - pS->ip;
    //    Vec3f pb = pQ->p[1] - pS->ip;
    //    Vec3f pc = pQ->p[2] - pS->ip;
    //    Vec3f pd = pQ->p[3] - pS->ip;
    //    Vec3f pq = pS->tp - pS->ip;

    //    //スカラー三重積の交換法則よりQP×P1使いまわす
    //    Vec3f crs = Vec3Cross(&pc, &pq);

    //    float u = Vec3Dot(&crs, &pa);
    //	  if(u >= 0)
    //	  {
    //	  	//ACB
    //	  	float v = -Vec3Dot(&crs, &pb);
    //	  	if(v< 0) return false;
    //	  	float w = Vec3ScalarTriple(&pq, &pb, &pa);
    //	  	if(w< 0)
    //	  	{
    //	  		return false;
    //	  	}
    //	  	//衝突位置は体積比による重心位置になる
    //	  	//*pC = (u*pb + v*pa + w*pc) / (u+v+w)+pS->ip;
    //	  }
    //	  else //(u>0)
    //	  {
    //	  	//ADC
    //	  	float v = Vec3Dot(&crs, &pd);
    //	  	if(v< 0) return false;
    //	  	float w = Vec3ScalarTriple(&pq, &pa, &pd);
    //	  	if(w< 0) return false;
    //	  	u = -u;
    //	  	//衝突位置は体積比による重心位置になる		
    //	  	//*pC = (u*pd + v*pa + w*pc) / (u+v+w)+pS->ip;
    //	  }

    //	  if( !IntersectSegmentPlane(pS, pQ->p[0], pQ->p[1], pQ->p[2] ))
    //	  	return false;

    //	  return true;
    //}

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

    //bool Collision::IntersectSegmentPlane( const Segment* pS, const Vec3f& d, const Vec3f& e, const Vec3f& f)
    //{
	//    Vec3f pn = Vec3Cross(&(e - d), &(f - d));
    //    float pd = Vec3Dot(&pn, &d);
    //
    //    Vec3f s = pS->tp - pS->ip;
    //    float t = (pd - Vec3Dot(&pn, &pS->ip)) / Vec3Dot(&pn, &s);
	//    if(t >= 0.0f && t <= 1.0f )
	//    {
	//	    Vec3f q = pS->ip + t * s;
	//	    return true;
	//    }
    //
	//    return false;
    //}

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