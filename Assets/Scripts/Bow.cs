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

    public float curPower;

    public float arrowGrav;

    public bool hasArrow;

    public Arrow3 CreateArrow()
    {
        var pos = transform.position + transform.forward * 0.3f + transform.right * 0.01f;
        var obj = Instantiate<GameObject>(arrowPrefab);
        obj.transform.position = pos;
        //obj.transform.rotation = transform.rotation * Quaternion.AngleAxis(90.0f, Vector3.right) ;
        obj.transform.rotation = transform.rotation;
        var arr = obj.GetComponent<Arrow3>();
        arr.calcData = CalculatedData.Create(this.transform.forward, curPower);
        arrow = arr;
        arrow.gameObject.transform.SetParent(gameObject.transform);
        hasArrow = true;
        return arr;
    }


    public void Shoot()
    {
        if(arrow != null)
        {
            arrow.gameObject.transform.parent = null;
            var data = new CalculatedData();
            data.dir = transform.forward;
            data.speed = curPower;
            data.startPos = transform.position;
            data.grav = arrowGrav;
               
            arrow.Shot( data );
            hasArrow = false;
        }
    }

    //set power( min ~ max )
    public void SetPower(float pow)
    {
        if      (pow < minPower) curPower = minPower;
        else if (pow > maxPower) curPower = maxPower;
        else                     curPower = pow;
    }

    //未完成
    public void UpdateDraw(float mov)
    {
        arrow.transform.position += -transform.forward * mov;
        curPower += 1f;
        if (curPower > maxPower) curPower = maxPower;
    }

}
