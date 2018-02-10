using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowString : MonoBehaviour
{

    [SerializeField]
    Transform head;
    public Transform Head{ get{ return head; } }

    [SerializeField]
    Transform tail;
    public Transform Tail { get { return tail; } }

    [SerializeField]
    Transform stringCenter;
    public Transform StringCenter { get { return stringCenter; } }

    [SerializeField]
    LineRenderer line;
    
    void Update()
    {

        line.SetPosition(0, head.position);
        line.SetPosition(1, stringCenter.position);
        line.SetPosition(2, tail.position);

    }


}
