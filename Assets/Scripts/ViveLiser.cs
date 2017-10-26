using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveLiser : MonoBehaviour
{

    [SerializeField]
    LineRenderer line;

    public float length;

    public Vector3[] postions;

    void Awake()
    {
        postions = new Vector3[2];
        postions[0] = transform.position;
        postions[1] = transform.position + transform.forward * length;
        line.material = new Material(Shader.Find("Mobile/Particles/Additive"));
    }


    void Start()
    {

    }

    void Update ()
    {
        postions[0] = transform.position;
        postions[1] = transform.position + transform.forward * length;
        line.SetPositions( postions );
    }
}
