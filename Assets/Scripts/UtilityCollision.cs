using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityCollision
{

    //intersect segment vs quad
    public static bool IntersectSegmentQuad(Vector3 p1, Vector3 p2, CollionQuad q)
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

}

[System.Serializable]
public class CollionQuad
{
    public Vector3 center;

    public Vector3 u;

    public Vector3 v;
}