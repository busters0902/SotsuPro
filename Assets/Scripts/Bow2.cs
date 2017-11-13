using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow2 : MonoBehaviour
{
    [SerializeField]
    GameObject body;

    [SerializeField]
    Transform head;
    public Transform Head { get { return head; } }

    [SerializeField]
    Transform tail;
    public Transform Tail { get { return tail; } }

    [SerializeField]
    Transform stringCenter;
    public Transform StringCenter { get { return stringCenter; } }

    [SerializeField]
    Transform stringBasePos;
    public Transform StringBasePos { get { return stringBasePos; } }

    [SerializeField]
    Transform arrowPlace;

    [SerializeField]
    public Arrow3 arrow;

    [SerializeField]
    GameObject arrowPrefab;

    public float arrowSetForward;

    [SerializeField]
    float maxPower;

    [SerializeField]
    float minPower;

    public float curPower;

    public float arrowGrav;

    public bool hasArrow;

    public Arrow3 CreateArrow()
    {
        var obj = Instantiate<GameObject>(arrowPrefab);
        var pos = transform.position + transform.forward * arrowSetForward;
        obj.transform.position = pos;
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
        if (arrow != null)
        {
            arrow.gameObject.transform.parent = null;
            var data = new CalculatedData();
            data.dir = transform.forward;
            data.speed = curPower;
            data.startPos = arrow.transform.position;
            data.grav = arrowGrav;

            arrow.Shot(data);
            hasArrow = false;
        }
    }

    //set power( min ~ max )
    public void SetPower(float pow)
    {
        if (pow < minPower) curPower = minPower;
        else if (pow > maxPower) curPower = maxPower;
        else curPower = pow;
    }

    //未完成
    public void UpdateDraw(float mov)
    {
        arrow.transform.position += -transform.forward * mov;
        curPower += 1f;
        if (curPower > maxPower) curPower = maxPower;
    }
}
