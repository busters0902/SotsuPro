using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    GameObject body;

    [SerializeField]
    public Arrow3 arrow;

    [SerializeField]
    GameObject arrowPrefab;

    [SerializeField]
    float maxPower;

    [SerializeField]
    float minPower;

    [SerializeField]
    public float curPower;

    public bool hasArrow;

    void Update()
    { 

    }

    public Arrow3 CreateArrow()
    {
        var pos = transform.position + transform.forward * 0.3f + transform.right * 0.01f;
        var obj = Instantiate<GameObject>(arrowPrefab);
        obj.transform.position = pos;
        //obj.transform.rotation = transform.rotation * Quaternion.AngleAxis(90.0f, Vector3.right) ;
        var arr = obj.GetComponent<Arrow3>();
        arr.calcData = CalculatedData.Create(this.transform.forward, curPower);

        arrow = arr;
        arrow.gameObject.transform.SetParent(gameObject.transform);

        hasArrow = true;

        //gameObject.transform
        return arr;
    }


    public void Shoot()
    {
        if(arrow != null)
        {
            //arrow.transform.position;
            //body.transform.position;
            arrow.gameObject.transform.parent = null;
            var data = new CalculatedData();
            data.dir = transform.forward;
            data.speed = curPower;
            data.startPos = transform.position;
            data.grav = 0.1f;

            
            arrow.Shot( data );
            hasArrow = false;
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
