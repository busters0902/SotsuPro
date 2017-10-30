using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    GameObject body;

    [SerializeField]
    public Arrow arrow;

    [SerializeField]
    GameObject arrowPrefab;

    [SerializeField]
    float maxPower;

    [SerializeField]
    float minPower;

    [SerializeField]
    float curPower;

    void Update()
    { 

    }

    public Arrow CreateArrow()
    {
        var pos = transform.position + transform.forward * 0.3f + transform.right * 0.01f;
        var obj = Instantiate<GameObject>(arrowPrefab);
        obj.transform.position = pos;
        obj.transform.rotation = transform.rotation * Quaternion.AngleAxis(90.0f, Vector3.right) ;
        var arr = obj.GetComponent<Arrow>();

        arrow = arr;
        arrow.gameObject.transform.SetParent(gameObject.transform);

        curPower = minPower;
        //gameObject.transform 
        return arr;
    }


    public void Shoot()
    {
        if(arrow != null)
        {
            //arrow.transform.position;
            //body.transform.position
            arrow.gameObject.transform.parent = null;
            arrow.Shot(transform.forward, curPower );
        }
    }

    public void UpdateDraw( Vector3 mov )
    {
        //arrow.transform.position += mov;

    }

    public void UpdateDraw(float mov)
    {
        arrow.transform.position += -transform.forward * mov;
        //arrow.transform.position
        curPower += 1f;
        if (curPower > maxPower) curPower = maxPower;
    }

}
