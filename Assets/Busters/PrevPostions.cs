using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimePostion
{
    public float time;
    public Vector3 pos;
}


public class PrevPostions : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    int prevNum;

    public List<TimePostion> postions = null;

    void Start()
    {
        Reset();
    }

    void Update()
    {
        var pos_ = target.position;
        var time_ = Time.deltaTime;
        var t = new TimePostion { time = time_, pos = pos_ };
        postions.Add(t);

        postions.RemoveAt(0);
    }

    public void Reset()
    {
        postions = new List<TimePostion>();
        for (int i = 0; i < prevNum; i++)
            postions.Add(null);
    }

}
