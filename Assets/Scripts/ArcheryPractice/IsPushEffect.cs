using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPushEffect : MonoBehaviour {
    
    void OnCollisionEnter(Collision collision)
    {
        Destroy(this);
    }
}
