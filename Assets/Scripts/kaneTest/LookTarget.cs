using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTarget : MonoBehaviour
{

    [SerializeField]
    Transform target;

    [SerializeField]
    SpriteRenderer sprite;

    void Update()
    {
        var targetVector = transform.position - target.transform.position;
        var cross = Vector3.Cross(targetVector, transform.forward);
        var targetVectorCross = Vector3.Cross(cross, targetVector);
        var angle2D = Mathf.Atan2(targetVectorCross.y, targetVectorCross.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle2D + 90);

        var angle = Vector3.Angle(transform.forward, -targetVector.normalized);
        //Debug.Log("kakudo" + angle.ToString());

        if (angle < 5)
        {
            //Debug.Log("入ってる");
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.05f);
        }
        else
        {
            //Debug.Log("入ってない");
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + 0.05f);
        }

    }
}
